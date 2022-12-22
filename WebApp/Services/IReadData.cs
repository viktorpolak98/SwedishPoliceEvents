using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Services
{
    /// <summary>
    /// Generic Interface to read data from unspecified source
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadData <T>
    {

        public Task<T> ReadData(String path);
    }
}
