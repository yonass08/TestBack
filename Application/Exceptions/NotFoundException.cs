using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entityName , object identifier):base($"The {entityName} with {identifier} not found"){}
        
    }
}