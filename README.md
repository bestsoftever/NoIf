# The goal

To write code which doesn't look that bad as usual.

## What is the problem?

Typical code is full of instructions responsible for error and exception handling, 
so the actuall flow is hidden and very often invisible. 

Example:

```csharp

try
{
    var user = _usersRepository.GetById(request.UserId);

    if (user is null)
    {
        var userData = Mapper.Map<User>(request.NewUser);
        user = _usersRepository.Add(userData);
    }
}
catch(DatabaseException dex)
{
    throw new InvalidUserException("Error creating user", dex);
}

try
{
}

```

I know, we can split this in smaller methods, but almost nobody does that, and it won't help much with 
visibility of the flow in the code.

## How it could look like?

```csharp
public Result<OrderId> PlaceOrder(OrderRequest orderRequest)
{
    return _customersRepository.GetById(orderRequest.UserId)
        .Then(customer => _customerDiscountsService.Calculate(customer))
        .Then(discount => _itemsMapper.MapItems(request.Items)
            .Then(items => _warehouseService.EnsureAvailability(items))
            .Then(items => _invoiceGenerator.Calculate(items, discount))
            .Then(invoice => _paymentProcessor.Send(invoice))
            .Then(paymentData => _warehouseSerbice.Send(user.Address))
        );
}
```

For me this code is much cleaner.

# Requirements

## Functional

1. We need a type which represents a result of an operation. Let's call it `Result<T>`.
1. We need a set of `Then` methods which allows to invoke next operation on `Result<T>` (with support to async code)
1. We need to be able to plug-in into the flow also fuctions which don't have return values. Classic `void` won't work. And no, `null` is not an option here.
1. Errors can be nested/composite.
1. We need methods which allows 

## Non-functional

1. It must be easy to use and easy to migrate from the existing codebase. (So even lazy people like me can use it.)
1. No time-wasting methods like:
    a. `return ResultFactory.CreateResultOf<OrderId>(...)` 
    b. `return Result.Of<OrderId>(...)` 
    c. `return new Result<OrderId>(...)`
    d. (and any other similar things)
2. No `result.IsError` - the goal of this library is to prevent people from using `if` statements. Such property is counterproductive.
2. No `result.IsValid` - it's even worse than `IsError`, because errors needs special handling, not valid results!
2. No `result.IsSuccess` - it's even worse than `IsValid`, programming has nothing to do with success.
2. No paramterless constructors available for users to prevent people from creting unintialized results.
2. Errors must go through the whole flow without re-mapping.

# The solution

Some people just write a simple class like that:

```csharp
public class ServiceResult<TData> : Either<ServiceResultError, TData>
{
    public ServiceResult(ServiceResultError error) : base(error)
    {
    }

    public ServiceResult(TData data) : base(data)
    {
    }

    public bool IsError => isLeft;
    public ServiceResultError Error => left;
    public TData Data => right;
}
```

And it violates the . 

Is here.

