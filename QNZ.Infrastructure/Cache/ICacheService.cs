using System;
using System.Collections.Generic;
using System.Text;

namespace QNZ.Infrastructure.Cache
{
    public interface ICacheService
    {
        object Get(string key);
        void Set(string key, object data, int cacheTime);
        bool IsSet(string key);
        void Invalidate(string key);
    }
}
