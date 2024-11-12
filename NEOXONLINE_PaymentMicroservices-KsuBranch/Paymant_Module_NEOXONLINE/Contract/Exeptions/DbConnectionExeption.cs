namespace Paymant_Module_NEOXONLINE.Contract.Exeptions
{
    public class DbConnectionExeption : Exception
    {
        public DbConnectionExeption(string? message) : base(message) { }

        public DbConnectionExeption(string? message, Exception? innerExeption) : base(message, innerExeption) { }
    }
}
