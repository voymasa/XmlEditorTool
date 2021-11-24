using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlEditorTool.Models;

namespace XmlEditorTool.ViewModels
{
    class ContentItemViewModel : ViewModelBase
    {
        public ContentItemModel ContentModel { get; protected set; }

        public string ContentHeader
        {
            get { return ContentModel.ContentHeader; }
            set
            {
                if (ContentModel.ContentHeader != value)
                {
                    ContentModel.ContentHeader = value;
                    RaisePropertyChanged(() => ContentHeader);
                }
            }
        }

        public string ContentValue
        {
            get { return ContentModel.ContentValue; }
            set
            {
                if (ContentModel.ContentValue != value)
                {
                    ContentModel.ContentValue = value;
                    if (ContentModel.DefaultValue == value)
                    {
                        IsDefaultValue = true;
                    }
                    else
                    {
                        IsDefaultValue = false;
                    }
                    RaisePropertyChanged(() => ContentValue);
                }
            }
        }

        public bool IsDefaultValue
        {
            get { return ContentModel.IsDefaultValue; }
            set
            {
                if (ContentModel.IsDefaultValue != value)
                {
                    ContentModel.IsDefaultValue = value;
                    RaisePropertyChanged(() => IsDefaultValue);
                }
            }
        }

        public ContentItemViewModel(ContentItemModel ContentModel)
        {
            this.ContentModel = ContentModel;
        }

        public KeyValuePair<string, string> GetContentHeaderValuePair(string? stringToAddToContentHeader, bool isKey)
        {
            return ContentModel.GetContentHeaderValuePair(stringToAddToContentHeader, isKey);
        }
    }
}
