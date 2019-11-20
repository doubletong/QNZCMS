using System;
using System.Collections.Generic;
using System.Text;

namespace SIG.Infrastructure.Helper
{
    public static class HostingEnvironment
    {
        public static bool m_IsHosted;

        static HostingEnvironment()
        {
            m_IsHosted = false;
        }

        public static bool IsHosted => m_IsHosted;
    }
}
