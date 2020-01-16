# NETStandardLibrary.Search

A library for building dynamic and flexible LINQ queries.

## Dependencies

* NETStandardLibrary.Linq

## Introduction

This is going to be a long one, but hopefully informative...

Typically when you write a LINQ query, it looks something like this:

```csharp
var results = db.MyCollection
    .Where(x => x.Quantity > 0)
    .Where(x => x.Size > 10)
    .ToList();
```

While this is great for a quick static query, when building searches we often want behavior more like this:

```csharp
var query = db.MyCollection.Where(x => x.CategoryID == fields.CategoryID);
if (fields.Quantity.HasValue)
    query = query.Where(x => x.Quantity == fields.Quantity.Value);
if (fields.Size.HasValue)
    query = query.Where(x => x.Size == fields.Size.Value);
...
var results = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
```

There are some problems we will run across while trying to maintain this:

* Every time a new search field is added a developer will need to implement a conditional statement in each query that will utilize that field.
* Each query that wants paginated data, or other information like total count, would need to implement that logic or use some shared logic a developer would need to build and maintain.
* If we want to get any extra information like total count, pagination, etc., we need to build models around that and implment additional logic.

This library serves to make that much easier with a flexible approach to building query parameters and executing search queries.

Another option is (or similar to) using a `Dictionary<string, object>` approach, like:

```csharp
var query = query.Where(x => x.CategoryID == categoryID);
foreach(Dictionary<string, object> field in fields)
{
    switch (field.Key)
    {
        case "Quantity":
            query = query.Where(x => x.Quantity == field.Value);
            break;
        case "Size":
            query = query.Where(x => x.Size == field.Value);
            break;
        ...
        default:
            throw new NotImplementedException();
    }
}
```

This also has drawbacks of needing to be maintained when querying new data. This library is looking to help solve that...at least a bit.