using Application.Builder;
using Application.Common.Interfaces;
using Application.Orders;
using Domain.Orders;
using Domain.Products;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Xunit;

namespace Api.Tests.Integration;

public class OrderObserverTests
{
    private readonly ILogger _loggerMock;
    private readonly OrderObserver _orderObserver;
    private readonly IObserver _observerMock;
    private readonly Order _order;
    private readonly OrderBuilder _orderBuilder;

    public OrderObserverTests()
    {
        _loggerMock = Substitute.For<ILogger>();
        _orderObserver = new OrderObserver(_loggerMock);
        _observerMock = Substitute.For<IObserver>();
        _orderBuilder = new OrderBuilder();
    }

    [Fact]
    public void AttachObserver_Success()
    {
        var orderNotifier = new OrderNotifier();
        orderNotifier.Attach(_observerMock);

        Assert.Contains(_observerMock, orderNotifier);
    }

    [Fact]
    public void DetachObserver_Success()
    {
        var orderNotifier = new OrderNotifier();
        orderNotifier.Attach(_observerMock);
        orderNotifier.Detach(_observerMock);

        Assert.DoesNotContain(_observerMock, orderNotifier);
    }

    [Fact]
    public void NotifyAdd_Success()
    {
        var orderNotifier = new OrderNotifier();
        orderNotifier.Attach(_observerMock);

        orderNotifier.NotifyAdd(_order);

        _observerMock.Received(1).Add(_order);
    }

    [Fact]
    public void NotifyUpdate_Success()
    {
        var orderNotifier = new OrderNotifier();
        orderNotifier.Attach(_observerMock);

        orderNotifier.NotifyUpdate(_order);

        _observerMock.Received(1).Update(_order);
    }

    [Fact]
    public void NotifyDelete_Success()
    {
        var orderNotifier = new OrderNotifier();
        orderNotifier.Attach(_observerMock);

        orderNotifier.NotifyDelete(_order);

        _observerMock.Received(1).Delete(_order);
    }
}