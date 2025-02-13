# NoIf

Write simpler to understand code.
It means - to avoid throwing exceptions in case of business errors and to avoid using `if` or `catch` statements to check 
if positive path can be continued.

# Requirements

## Functional

1. Type which represents a result of an operation. Let's call it `Result<T>`.
1. Set of `Then` methods which allows to invoke next operation on `Result<T>` (with support to async code)
1. Support also fuctions which don't return values. Classic `void` won't work. And no, `null` is not an option here.
1. Errors can be nested/composite.
1. Methods which allows to filter out errrors and convert them to valid results and vice versa. (Very often an error returned from one method is handled in another one and doesn't have to be returned as an error to the upper layer.)

## Non-functional

1. It must be easy to use and easy to migrate from the existing codebase.
1. No time-wasting methods like:
    a. `return AbstractResultFactoryBeanImpl.CreateResultOf<OrderId>(...)` 
    b. `return Result.Of<OrderId>(...)` 
    c. `return new Result<OrderId>(...)`
    d. ... or any other similar things
2. No `result.IsError` - the goal of this library is to prevent people from using `if` statements. Such property is counterproductive.
2. No `result.IsValid` - it's even worse than `IsError`, because errors needs special handling, not valid results!
2. No `result.IsSuccess` - it's even worse than `IsValid`, programming has nothing to do with success.
2. No parameterless constructors available for users to prevent people from creting unintialized results.
2. Errors must go through the whole flow without re-mapping.

# The solution

```csharp
var result = GetRandomPetFromOracle()
	.Act<Error>(e => errorMessage = e.Message)
	.Act<Dog>(d => isPies = true)
	.Swap<ThisParrotIsDeadError>(e => new Cat("kotek"))
	.Swap<Dog>(d => new Cat(d.Name))
	.Then(c => TestService.ToUpperCase($"{c.GetType().Name} is {c.Name}"));
```

# Migration

## Introducing Result\<T>

Let's take a method:

```csharp
public static string ReverseString(string input)
{
	if (string.IsNullOrWhiteSpace(input))
	{
		throw new ArgumentException(nameof(input));
	}

	return new string(input.Reverse().ToArray());
}
```

With `NoIf` we  need to replace throwing exceptions with returning Error:

```csharp
public static Result<string> ReverseString(string input)
{
	if (string.IsNullOrWhiteSpace(input))
	{
		return new Error(ErrorMessage);
	}

	return new string(input.Reverse().ToArray());
}
```

## Flow simplification

We can convert this:

```csharp
public string ProcessAnimal(Guid id)
{
    Animal animal = null!;

    try
    {
	    animal = GetPetFromOracle(id);
    }
    catch (ThisParrotIsDeadException pe)
    {
	    errorMessage = pe.Message;
	    animal = new Cat("kotek");
    }
    catch (Exception e)
    {
	    errorMessage = e.Message;
    }

    if (animal is Dog d)
    {
	    isPies = true;
	    animal = new Cat(d.Name);
    }

    var result = TestService.ToUpperCase($"{animal.GetType().Name} is {animal.Name}");
	return result;
}
```

to this:

```csharp
public Result<string> ProcessAnimal(Guid id)
{
	return GetPetFromOracle(id)
		.Act<Error>(e => errorMessage = e.Message)
		.Act<Dog>(d => isPies = true)
		.Swap<ThisParrotIsDeadError>(e => new Cat("kotek"))
		.Swap<Dog>(d => new Cat(d.Name))
		.Then(c => TestService.ToUpperCase($"{c.GetType().Name} is {c.Name}"));
}
```

Of course, the `GetPetFromOracle` method must return `Result<Animal>` and use errors instead of exceptions, but this is an easy migration as shown before.