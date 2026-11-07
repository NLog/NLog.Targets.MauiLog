namespace Java.Lang;

public class Throwable : System.Exception
{
    private Throwable(System.Exception exception)
        : base(exception.Message, exception)
    {
        SourceException = exception;
    }

    public System.Exception SourceException { get; }

    public static Throwable FromException(System.Exception exception) => new(exception);
}