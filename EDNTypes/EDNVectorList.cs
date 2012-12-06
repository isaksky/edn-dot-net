﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDNTypes
{


    public class EDNList : IEnumerable<object>, IEDNEnumerable, IEDNPrintable
    {
        private List<object> ednList;

        public EDNList(IEnumerable<object> coll)
        {
            ednList = new List<object>(coll);
        }

        public int IndexOf(object item)
        {
            return ednList.IndexOf(item);
        }

        public object this[int index]
        {
            get
            {
                return ednList[index];
            }
        }

        public bool Contains(object item)
        {
            return ednList.Contains(item);
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
            ednList.CopyTo(array, arrayIndex);
        }

        public int Count()
        {
            return ednList.Count; 
        }

        #region IEnumerable<object> Members

        public IEnumerator<object> GetEnumerator()
        {
            return ednList.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ednList.GetEnumerator();
        }

        #endregion

        public override int GetHashCode()
        {
            return Utils.additionHashCode(ednList);
        }

        public override bool Equals(object obj)
        {
            return InternalEquals<EDNList>(obj);
        }

        public  bool InternalEquals<T>(object obj)
        {
            if (obj == null)
                return false;
            else if (ReferenceEquals(this, obj))
                return true;
            else if (obj.GetType() != typeof(T))
                return false;
            else if (obj.GetHashCode() != this.GetHashCode())
                return false;
            else
            {
                IEDNEnumerable e = (IEDNEnumerable)obj;

                if (e.Count() != this.Count())
                    return false;

                var enum1 = e.GetEnumerator();
                var enum2 = this.GetEnumerator();

                while (enum1.MoveNext() && enum2.MoveNext())
                {
                    if (ReferenceEquals(enum1.Current, enum2.Current))
                        continue;
                    else if (enum1.Current != null && !enum1.Current.Equals(enum2.Current))
                        return false;
                    else if (enum1.Current == null && enum2.Current != null)
                        return false;
                }

                return true;
            }
        }

        public static bool operator ==(EDNList obj1, object obj2)
        {
            return obj1.Equals(obj2);
        }

        public static bool operator !=(EDNList obj1, object obj2)
        {
            return !obj1.Equals(obj2);
        }

        #region IEDNPrintable Members

        public virtual string PrintEDN(IPrintHandler handler)
        {
            return Utils.WritePrintableToString(this, handler);
        }

        public virtual void PrintEDN(System.IO.Stream stream, IPrintHandler handler)
        {
            stream.Write(Utils.openListBytes, 0, Utils.openListBytes.Length);
            handler.handleEnumerable(this, stream);
            stream.Write(Utils.closeListBytes, 0, Utils.closeListBytes.Length);
        }

        #endregion
    }


    public class EDNVector : EDNList
    {
        public EDNVector(IEnumerable<object> coll) : base(coll)
        {
        }

        public override string PrintEDN(IPrintHandler handler)
        {
            return Utils.WritePrintableToString(this, handler);
        }

        public override void PrintEDN(System.IO.Stream stream, IPrintHandler handler)
        {
            stream.Write(Utils.openVectorBytes, 0, Utils.openVectorBytes.Length);
            handler.handleEnumerable(this, stream);
            stream.Write(Utils.closeVectorBytes, 0, Utils.closeVectorBytes.Length);
        }

        public override bool Equals(object obj)
        {
            return InternalEquals<EDNVector>(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
