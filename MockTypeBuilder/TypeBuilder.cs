using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MockTypeBuilder.Constraints;

namespace MockTypeBuilder
{
    public class TypeBuilder<TBuildType> where TBuildType : new()
    {
        private List<TBuildType> _content;
        private List<TBuildType> _temporaryContent;
        private TBuildType _currentItem;

        private bool _singleEdit;

        private Dictionary<string, StringConstraints> _propertyStringContraints;
        private Dictionary<string, NumberConstraints> _propertyNumberContraints;
        private Dictionary<string, DateConstraints> _propertyDateContraints;

        private List<StringConstraints> _typeStringContraints;
        private List<NumberConstraints> _typeNumberContraints;
        private List<DateConstraints> _typeDateContraints;

        /// <summary>
        /// Returns the current state as a single item (last created/edited single item) or creates a new single item with random values if none exist
        /// </summary>
        /// <returns>Built item of given type</returns>
        public TBuildType BuildSingle()
        {
            if (_currentItem == null)
            {
                CreateSingle();
            }

            return _currentItem;
        }

        /// <summary>
        /// Returns the current state as a list or creates a new list with one item with random values if none exist
        /// </summary>
        /// <returns>Built list of given type</returns>
        public List<TBuildType> BuildList()
        {
            StageItemsCurrentlyInEdit();
            if (_content.Any()) return _content;

            CreateMultiple(1);
            _content.AddRange(_temporaryContent);
            return _content;
        }

        /// <summary>
        /// Creates a single item of provided type populated with random values.
        /// First call will not initialize a list but can be called mutiple to create a list of items.
        /// Any constraints added at the time of calling the method will affect the data.
        /// </summary>
        /// <returns></returns>
        public TypeBuilder<TBuildType> CreateSingle()
        {
            if (_currentItem != null)
            {
                if (_content == null)
                {
                    _content = new List<TBuildType> {_currentItem};
                }
                else
                {
                    _content.Add(_currentItem);
                }
            }

            _currentItem = new TBuildType();
            _singleEdit = true;
            return this;
        }

        /// <summary>
        /// Creates a list of items or adds multiple items to an existing list.
        /// Each item will have distinct random values. 
        /// Any constraints added at the time of calling the method will affect the data.
        /// All items created by a single call of this method will be selected for editing with other methods such as the WithProperty method
        /// </summary>
        /// <param name="noOfItems">The number of new items to create</param>
        /// <returns></returns>
        public TypeBuilder<TBuildType> CreateMultiple(int noOfItems)
        {
            StageItemsCurrentlyInEdit();

            _temporaryContent = new List<TBuildType>();

            for (var i = 0; i < noOfItems; i++)
            {
                var item = new TBuildType();
                _temporaryContent.Add(item);
            }

            _singleEdit = false;
            return this;
        }

        /// <summary>
        /// Updates the given property with a user-defined value that will replace the random value if specific tests need to carried out.
        /// If multiple items were created using a the CreateMutiple(int noOfItems) method, all items created on that single method call are selected by default and the properties of
        /// all selected items will update. Use the index parameter to select speciic items. 
        /// If multiple items were created with multiple calls to CreateSingle() method, only the item created with the last call will be selected for update.
        /// </summary>
        /// <typeparam name="T">The Type of property</typeparam>
        /// <param name="propertyName">name of property to update</param>
        /// <param name="value">The value to update the property with</param>
        /// <param name="index">Used to select specific items if mutiple items are selected. All selected items will be used if left null</param>
        /// <returns></returns>
        public TypeBuilder<TBuildType> WithProperty<T>(string propertyName, T value, List<int> index = null)
        {
            if (_singleEdit)
            {
                UpdateProperty(_currentItem, value, propertyName);
            }
            else
            {
                for (var i = 0; i < _temporaryContent.Count; i++)
                {
                    if (index == null || index.Contains(i))
                    {
                        UpdateProperty(_temporaryContent[i], value, propertyName);
                    }
                }
            }
            return this;
        }

        /// <summary>
        /// Adds a constraint to all string properties based on constraint arguments set in the Stringcontraints object. 
        /// By default, the constraint will only affect items created after the constraint is set. Set the regenerateValues parameter to true
        /// to update all existing items to meet the constraint.
        /// Optionally, the propertyName parameter can be set to limit the constraint to the specified property, by default all string properties will be affected
        /// </summary>
        /// <param name="constraint">Constraint arguments object</param>
        /// <param name="propertyName">name of property to add the constraint to</param>
        /// <param name="regenerateValues">Whether or not existing items should be updated (optional)</param>
        /// <returns></returns>
        public TypeBuilder<TBuildType> AddConstraint(StringConstraints constraint, string propertyName = null, bool regenerateValues = false)
        {
            if (propertyName != null)
            {
                _propertyStringContraints.Add(propertyName, constraint);
                if (regenerateValues) RePopulateByProperty(propertyName);
            }
            else
            {
                _typeStringContraints.Add(constraint);
                if (regenerateValues) RePopulateByType("String");
            }
            return this;
        }

        /// <summary>
        /// Adds a constraint to all string properties based on constraint arguments set in the Stringcontraints object. 
        /// By default, the constraint will only affect items created after the constraint is set. Set the regenerateValues parameter to true
        /// to update all existing items to meet the constraint.
        /// Optionally, the propertyName parameter can be set to limit the constraint to the specified property, by default all numeric properties will be affected
        /// </summary>
        /// <param name="constraint">Constraint arguments object</param>
        /// <param name="propertyName">name of property to add the constraint to</param>
        /// <param name="regenerateValues">Whether or not existing items should be updated</param>
        /// <returns></returns>
        public TypeBuilder<TBuildType> AddConstraint(NumberConstraints constraint, string propertyName = null, bool regenerateValues = false)
        {
            if (propertyName != null)
            {
                _propertyNumberContraints.Add(propertyName, constraint);
                if (regenerateValues) RePopulateByProperty(propertyName);
            }
            else
            {
                _typeNumberContraints.Add(constraint);
                if (regenerateValues) RePopulateByType("Number");
            }
            return this;
        }

        /// <summary>
        /// Adds a constraint to all date properties based on constraint arguments set in the DateConstraints object. 
        /// By default, the constraint will only affect items created after the constraint is set. Set the regenerateValues parameter to true
        /// to update all existing items to meet the constraint.
        /// Optionally, the propertyName parameter can be set to limit the constraint to the specified property, by default all DateTime properties will be affected
        /// </summary>
        /// <param name="constraint">Constraint arguments object</param>
        /// <param name="propertyName">name of property to add the constraint to</param>
        /// <param name="regenerateValues">Whether or not existing items should be updated</param>
        /// <returns></returns>
        public TypeBuilder<TBuildType> AddConstraint(DateConstraints constraint, string propertyName = null, bool regenerateValues = false)
        {
            if (propertyName != null)
            {
                _propertyDateContraints.Add(propertyName, constraint);
                if(regenerateValues) RePopulateByProperty(propertyName);
            }
            else
            {
                _typeDateContraints.Add(constraint);
                if (regenerateValues) RePopulateByType("Date");
            }
            return this;
        }

        #region private methods
        private void Create()
        {
            //TODO: create a new current item with random values
        }

        private static void UpdateProperty<T>(TBuildType updateObject, T value, string propertyName)
        {
            PropertyInfo property = updateObject.GetType().GetRuntimeProperty(propertyName);
            property.SetValue(updateObject, value);
        }

        private void StageItemsCurrentlyInEdit()
        {
            if (_content == null)
            {
                _content = new List<TBuildType>();
            }

            if (_temporaryContent != null && _temporaryContent.Any())
            {
                _content.AddRange(_temporaryContent);
            }

            if (_currentItem != null)
            {
                _content.Add(_currentItem);
            }
        }

        private void RePopulateByProperty(string propertyName)
        {
            //TODO: stage current items, move content to temp list, update temp list
        }

        private void RePopulateByType(string typeName)
        {
            //TODO: stage current items, move content to temp list, update temp list
        }
        #endregion
    }
}
