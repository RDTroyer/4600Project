using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace _4600Project
{
    public class BaseModel : INotifyPropertyChanged
    {
        /// <summary>
        /// The following is a default constructor for BaseModel
        /// </summary>
        public BaseModel()
        {

        }

        /// <summary>
        /// The following is used to validate any changes made to a property
        /// 
        /// Precondition: Checks if the property values are equal
        /// Postcondition: Changes the condition of the property if any change is needed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="backingStore">reference to backup</param>
        /// <param name="value">value</param>
        /// <param name="propertyName">the name of the property</param>
        /// <param name="onChanged">where it has been changed</param>
        /// <returns></returns>
        protected bool SetProperty<T>(
            ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)

        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();

            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
