using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Practices.Unity.Configuration.ConfigurationHelpers;
using Prism.Mvvm;

namespace GrandadAudioPlayer.Utils.Prism
{
    public abstract class ErrorCheckingBindableBase : BindableBase, INotifyDataErrorInfo
    {

        private Dictionary<string, List<string>> _propertyErrors = new Dictionary<string, List<string>>();

        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName == null) return null;

            _propertyErrors.TryGetValue(propertyName, out var errorList);
            return errorList;

        }

        public bool HasErrors
        {
            get
            {
                var propertyErrorCount = _propertyErrors.Values.FirstOrDefault(r => r.Count > 0);
                return propertyErrorCount != null;
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        private void _performValidation()
        {
            LocalValidation(ref _propertyErrors);

            foreach (var property in _propertyErrors.Keys)
            {
                var errorList = _propertyErrors.GetOrNull(property);

                if (errorList?.Count > 0)
                {
                    OnPropertyErrorsChanged(property);
                }
            }
        }

        protected abstract void LocalValidation(ref Dictionary<string, List<string>> propertyErrors);

        public void Validate()
        {
            _performValidation();
        }

        private void OnPropertyErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }


        /// <inheritdoc />
        ///
        /// Overriden to allow the calling of the validate function
        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);
            Validate();
        }
    }
}
