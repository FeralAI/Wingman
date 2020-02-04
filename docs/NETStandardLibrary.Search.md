# Wingman.Search

A library for building dynamic and flexible LINQ queries.

## Dependencies

* Wingman.Common
* LINQKit.Core

## Introduction

This is going to be a long one, but hopefully informative...

Typically when you write a LINQ query, it looks something like:

```c#
var results = db.MyCollection
    .Where(x => x.Quantity > 0)
    .Where(x => x.Size > 10)
    .ToList();
```

While this is great for a quick static query, when building searches we often want behavior more like:

```c#
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
* If we want to get any extra information like total count, pagination, etc., we need to build models around that and implment additional logic.

Another approach could be something like using an extension method and a `Dictionary<string, object>` like:

```c#
public static IQueryable<Item> Search(this IQueryable<Item> query, Dictionary<string, object> fields)
{
    foreach (KeyValuePair<string, object> field in fields)
    {
        switch (field.Key)
        {
            case "Quantity":
                query = query.Where(x => x.Quantity == (int)field.Value);
                break;
            case "Size":
                query = query.Where(x => x.Size == (int)field.Value);
                break;
            ...
            default:
                throw new NotImplementedException();
        }
    }

    return query;
}
```

This approach is a bit more flexible allowing execution of arbitrary searches, however it still needs to be
maintained when adding new fields, would need to be implemented for each table to be queried, and we still
don't have control over the operators in the `.Where()` clauses.

## A Solution

What if, I don't know, we could send an arbitrary list of fields, operators and values to an `IQueryable`
object and magically have your parameters applied to the query?

```c#
var searchFields = new List<SearchField>
{
    new SearchField
    {
        Name = "FirstName",
        Value = "Bob",
        ValueType = typeof(string),
        Operator = WhereClauseType.Equal
    },
    new SearchField
    {
        Name = "Age",
        Value = 25,
        ValueType = typeof(int),
        Operator = WhereClauseType.GreaterThanOrEqual
    },
};
var searchFieldList = new SearchFieldList(searchFields, WhereClauseOperator.AND);
var parameters = new SearchParameters
{
    Fields = searchFieldList,
    OrderBys = new OrderByClauseList("LastName DESC"),
    Page = 1,
    PageSize = 10
};
SearchResults<User> searchResults = db.Users.Search(parameters);
Console.WriteLine(searchResults.HasPaging);
Console.WriteLine(searchResults.TotalCount);
Console.WriteLine(searchResults.TotalPages);
// searchResults.Results
```


## Links

<https://www.c-sharpcorner.com/UploadFile/c42694/dynamic-query-in-linq-using-predicate-builder/>
<http://www.albahari.com/nutshell/predicatebuilder.aspx>
