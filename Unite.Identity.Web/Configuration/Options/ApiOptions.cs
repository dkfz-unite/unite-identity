﻿using System.Text;

namespace Unite.Identity.Web.Configuration.Options;

public class ApiOptions
{
    public byte[] Key
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_API_KEY");

            if (option == null)
                throw new ArgumentNullException("'UNITE_API_KEY' environment variable has to be set");

            if (option.Length != 32)
                throw new ArgumentOutOfRangeException("'UNITE_API_KEY' environment variable has to be a 32 bit string");

            return Encoding.ASCII.GetBytes(option);
        }
    }
}
