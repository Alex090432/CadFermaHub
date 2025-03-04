using System.Reflection;

namespace CadFermaHub.Service
{
    public interface IClassConverter
    {
        void Convert(object from, object to);
    }
    public class ClassConverter : IClassConverter
    {
        public void Convert(object from, object to)
        {
            Type tFrom = from.GetType();
            Type tTo = to.GetType();
            PropertyInfo[] pFrom = tFrom.GetProperties();
            foreach (PropertyInfo property in pFrom)
            {
                PropertyInfo propertyTo = tTo.GetProperty(property.Name);
                if (property.CanRead && propertyTo.CanWrite && property.PropertyType.Equals(propertyTo.PropertyType))
                    propertyTo.SetValue(to, property.GetValue(from));
            }
        }
    }
}
