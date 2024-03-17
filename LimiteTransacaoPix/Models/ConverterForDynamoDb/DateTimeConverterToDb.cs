using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace LimiteTransacaoPix.Models.ConvertToString
{
    public class DateTimeConverterToDb : IPropertyConverter
    {
        public object FromEntry(DynamoDBEntry entry)
        {
            if (entry == null || !(entry is Primitive))
            {
                throw new ArgumentOutOfRangeException();
            }

            var primitive = entry as Primitive;
            var dateTimeString = primitive.AsString(); 
            var dateTime = DateTime.Parse(dateTimeString); 

            return dateTime;
        }


        public DynamoDBEntry ToEntry(object value)
        {
            if (value == null || !(value is DateTime))
            {
                throw new ArgumentOutOfRangeException();
            }

            var dateTime = (DateTime)value;
            var dateTimeString = dateTime.ToString("o");

            return new Primitive(dateTimeString);
        }

    }
}
