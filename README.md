# NoIf

Write easier to understand code.
Or at least try to avoid throwing exceptions in case of errors and using `if` or `catch` statements to check 
if a happy path can be continued.

# Features

1. Type which represents a result of an operation - `Result<T>`. The variable of that type can store either data of type `T` or an `Error`.
1. `Error` - as base cless for errors returned from methods. It support nesting/aggregate errors.
1. Set of `Then` methods which allows to invoke next operation on `Result<T>` (also with overrides which support async code)
1. `Result.None` - to represent the lack of a value returned from a method.
1. Set of `Act<T>` methods - which allows to invoke an action if the result of the previous operation is `T`.
1. Set of `Swap<T>` methods - which allows to swap the result type if the result of the previous operation is `T`.

## What is not implemented

1. Methods like:
    a. `return AbstractResultFactoryBeanImpl.CreateResultOf<string>(...)` 
    b. `return Result.Of<string>(...)` 
    c. `return new Result<string>(...)`
1. No `result.IsError` - the goal of this library is to prevent people from using `if` statements, thus such property would make no sense.
1. No `result.IsValid` or `result.IsSuccess` - it's even worse than `IsError`, because errors needs special handling, not valid results!
1. No parameterless constructors available for users to prevent from creating unintialized results.

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

With `NoIf` we only need to replace throwing exceptions with returning Error:

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

No need to change valid cases, wraping it in `Result<T>`.

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