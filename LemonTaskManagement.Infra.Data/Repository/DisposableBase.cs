using System;

namespace LemonTaskManagement.Infra.Data.Repository;

public abstract class DisposableBase : IDisposable
{
    private bool _disposed;

    protected virtual void PreDispose()
    {
    }

    public virtual void Dispose()
    {
        if (!_disposed)
        {
            PreDispose();
            _disposed = true;
        }
    }
}
