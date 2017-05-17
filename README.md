# MockTypeBuilder
A fluid type builder .net standard library that uses generics to build mock objects for unit testing

## Setting up
Will end up as a nuget package, but as of yet it's just a .net standard class library on github as it is still a work in progress

## Usage (will be updated as more features are added)
### Creating a single item
Example type to create:
```
public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

By calling build straightaway without creating anything will generate 1 default item:
```
var builder = new TypeBuilder<Person>();
Person person = builder.BuildSingle();
```

But if you need more control over what's being created, use CreateSingle() Method. Each time it's called the builder will create a new item and finally you can use BuildList() or BuildSingle() to return either a list of all created items or the last single item created respectively.
```
var builder = new TypeBuilder<Person>();
builder.CreateSingle()
    .WithProperty("Name", "Gaff")
    .WithProperty("Id", 45);

Person person = builder.BuildSingle();
```

Result (returns a Person object):
```
{
    "Name": "Gaff",
    "Id" :45
}
```

To chain multiple items:
```
var builder = new TypeBuilder<Person>();
builder.CreateSingle()
    .WithProperty("Name", "Gaff")
    .WithProperty("Id", 45);
    .CreateSingle()
    .WithProperty("Name", "Hess")
    .WithProperty("Id", 46)
    .CreateSingle()
    .WithProperty("Id", 47)

Person personSingle = builder.BuildSingle();
List<Person> personMultiple = builder.BuildList();
```

Result for personSingle:
```
{
    "Name": null,
    "Id" :47
}
```

Result for personMultiple:
```
[
    {
        "Name": "Gaff",
        "Id" :45
    },
    {
        "Name": "Hess",
        "Id" :46
    },
    {
        "Name": null,
        "Id" :47
    }
]
```

### Creating bulk items
If you want to test a massive with a lot of items, you probably wouldn't want to call CreateSingle() a hundred times. You can use CreateMultiple(int noOfItems) to generate mutiple items. All items generated using this method is also selected by default for editing which mean you can do things like update a property of all items created with one call to WithProperty().
```
var builder = new TypeBuilder<Person>();
builder.CreateMultiple(5)
    .WithProperty("Name", "Gaff")
    .WithProperty("Id", 45);

List<Person> persons = builder.BuildList();
```

Result (Returns a list of Person objects)
```
[
    {
        "Name": "Gaff",
        "Id" :45
    },
    {
        "Name": "Hess",
        "Id" :45
    },
    {
        "Name": "Gaff",
        "Id" :45
    },
    {
        "Name": "Gaff",
        "Id" : 45
    },
    {
        "Name": "Gaff",
        "Id" : 45
    }
]
```

If you need to only update specific items on the list, you can also a list of indexes for those items
```
var builder = new TypeBuilder<Person>();
builder.CreateMultiple(5)
    .WithProperty("Id", 12, new List<int> { 1, 2, 4 });

List<Person> persons = builder.BuildList();
```

Result:
```
[
    {
        "Name": null,
        "Id" :12
    },
    {
        "Name": null,
        "Id" :12
    },
    {
        "Name": null,
        "Id" :null
    },
    {
        "Name": null,
        "Id" : 12
    },
    {
        "Name": null,
        "Id" : null
    }
]
```

### Upcoming features
 - Currently if property is not specified, fields are null. However, as I originally planned, it should generate some default values either randomly or user defined. User should also be able to set some constraints to the generated values.
 - Ability to generate types of preset real-world values such as sequential Ids, credit card numbers, addresses etc.