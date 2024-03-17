using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;

namespace LimiteTransacaoPix.Models.ConvertToString
{
    public class DecimalConverterToDb : IPropertyConverter
    {
        public object FromEntry(DynamoDBEntry entry)
        {
            Primitive primitive = entry as Primitive;
            if (primitive is null || !(primitive.Value is String) || string.IsNullOrEmpty((string)primitive.Value))
            {
                throw new ArgumentOutOfRangeException();
            }
            var decimalString = primitive.AsString(); 
            var dec = Convert.ToDecimal(decimalString); 

            return dec;
        }


        public DynamoDBEntry ToEntry(object value)
        {
            if (value is null)
            {
                throw new ArgumentOutOfRangeException();
            }

            
            var dec = (decimal)value;
            var decimalString = dec.ToString();

            return new Primitive(decimalString);
        }

    }
}
