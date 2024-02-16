using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator.GUI
{
    /// <summary>
    /// Represents a shared asset within a family environment, such as a car or a lawnmower.
    /// This class is responsible for managing the state of an asset, including its availability.
    /// 
    /// Each asset has a name, a description, and a status that indicates whether it is currently borrowed.
    /// This class implements INotifyPropertyChanged to allow UI elements to react to property changes.
    /// 
    /// @Author Liza Danielsson.
    /// </summary>
    public class Asset : INotifyPropertyChanged
    {
        private string _name;
        private string _description;
        private string _status;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        private readonly object _lock = new object();

        public Asset(string name, string description)
        {
            Name = name;
            Description = description;
            Status = "Available"; // Default status
        }


        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Raises the PropertyChanged event for the specified property name.
        /// </summary>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        /// <summary>
        /// Marks the asset as borrowed by the specified member, if it is available.
        /// </summary>
        public void Borrow(string memberName)
        {
            lock (_lock)
            {
                if (Status == "Available")
                {
                    Status = $"Borrowed by {memberName}";
                }
            }
        }


        /// <summary>
        /// Marks the asset as available, indicating it has been returned.
        /// </summary>
        public void Return()
        {
            lock (_lock)
            {
                Status = "Available";
            }
        }
    }

}
