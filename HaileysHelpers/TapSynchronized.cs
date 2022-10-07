/// <summary>
/// Wrap a value in a thread-synchronized context
/// </summary>
/// <typeparam name="T">The type of the value you wish to synchronize</typeparam>
/// <remarks>This uses a single mutex for the value. As a result, trying to use this in a tight loop has a decent amount of overhead.</remarks>
public class TapSynchronized<T>
{
    private T _inner;
    private Mutex _mutex = new();

    /// <summary>
    /// The callback to execute with your data
    /// </summary>
    /// <typeparam name="TResult">A pass-through return of whatever you return in the inner function</typeparam>
    public delegate TResult? WithValueDelegate<out TResult>(ref T ctx);
    
    /// <summary>
    /// Create a Synchronized value
    /// </summary>
    /// <param name="value">Your value or handle</param>
    /// <remarks>You should not use the value after providing it here. Only access your value via this wrapper.</remarks>
    public TapSynchronized(ref T value)
    {
        _inner = value;
    }
    
    /// <summary>
    /// Create a Synchronized value
    /// </summary>
    /// <param name="value">Your value or handle</param>
    /// <remarks>You should not use the value after providing it here. Only access your value via this wrapper.</remarks>
    public TapSynchronized(T value)
    {
        _inner = value;
    }


    /// <summary>
    /// Execute arbitrary code with a handle to the wrapped value.
    /// </summary>
    /// <param name="cb">The code to execute</param>
    /// <typeparam name="TResult">The type returned in your delegate</typeparam>
    /// <returns>The passed through value returned in your delegate</returns>
    /// <remarks>Defaults to 1 second of wait time. If this time expires, the callback will never be called, and return will be null.</remarks>
    public TResult? WithValue<TResult>(WithValueDelegate<TResult> cb)
    {
        return WithValue(cb, 1000);
    }

    /// <summary>
    /// Execute arbitrary code with a handle to the wrapped value.
    /// </summary>
    /// <param name="cb">The code to execute</param>
    /// <param name="timeout">The timeout to wait for your mutex</param>
    /// <typeparam name="TResult">The type returned in your delegate</typeparam>
    /// <returns>The passed through value returned in your delegate</returns>
    /// <remarks>If this time expires, the callback will never be called, and return will be null.</remarks>
    public TResult? WithValue<TResult>(WithValueDelegate<TResult> cb, int timeout)
    {
        if (!_mutex.WaitOne(timeout))
        {
            // We failed to lock the mutex in time, just bail.
            return default;
        }

        TResult? ret = default;
        Exception? ex = null;

        // SAFETY: This is wrapped in a try-catch to ensure we always release the mutex, even in cases of failure within
        // user code.
        try
        {
            ret = cb(ref _inner);
        }
        catch (Exception e)
        {
            ex = e;
        }
        finally
        {
            _mutex.ReleaseMutex();
        }

        // Re-throw the exception, if one has occurred
        if (ex != null)
        {
            throw ex;
        }
        
        return ret;
    }
}