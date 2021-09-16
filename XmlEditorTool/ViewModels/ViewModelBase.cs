using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace XmlEditorTool.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public string MacroName { get; set; }
        //public string ContentErrorMsg { get; }
        public Dictionary<string, string> AttributeValueDictionary { get; set; }

        public virtual Dictionary<string, string> CreateAttributeValueDictionaryFromData() { return AttributeValueDictionary; }

        #region Constructor
        protected ViewModelBase()
        {

        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Event Handlers
        /// <summary>
        /// Get name of Property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetPropertyName<T>(Expression<Func<T>> e)
        {
            var member = (MemberExpression)e.Body;
            return member.Member.Name;
        }

        /// <summary>
        /// Raise when property value propertycahnged or override propertychange
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyExpression"></param>
        protected virtual void RaisePropertyChanged<T>
            (Expression<Func<T>> propertyExpression)
        {
            RaisePropertyChanged(GetPropertyName(propertyExpression));
        }

        /// <summary>
        /// Raise when property value propertychanged
        /// </summary>
        /// <param name="propertyName"></param>
        protected void RaisePropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
