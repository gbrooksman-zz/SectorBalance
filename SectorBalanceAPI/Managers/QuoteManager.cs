using System;
using System.Collections.Generic;
using Dapper;
using Npgsql;
using SectorBalanceShared;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Dapper.FastCrud;

namespace SectorBalanceAPI
{
    public class QuoteManager : BaseManager
    {

        public QuoteManager(IMemoryCache _cache, IConfiguration _config) : base(_cache, _config)
        {

        }

        public Quote GetBySymbol(string symbol)
        {
            Quote quote = new Quote();

            using (NpgsqlConnection db = new NpgsqlConnection(base.connString))
            {
               quote =  db.Find<Quote>().Where(q => q.Symbol == symbol).FirstOrDefault();
            }

            return quote;
        }

       public Quote Add(Quote quote)
        {        
            if (GetBySymbol(quote.Symbol) == null) 
            {
                using (NpgsqlConnection db = new NpgsqlConnection(base.connString))
                {
                    db.Insert(quote);
                }
            }
            else
            {
                quote = new Quote();
            }
             
            return quote;
        }


       public Quote Delete(Quote quote)
       {        
            if (GetBySymbol(quote.Symbol) != null) 
            {
                using (NpgsqlConnection db = new NpgsqlConnection(base.connString))
                {
                    db.Delete(quote);
                }
            }
            else
            {
                quote = new Quote();
            }
             
            return quote;
        }

        
    }
}
