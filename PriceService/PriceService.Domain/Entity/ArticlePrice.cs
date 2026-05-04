using System;
using System.Collections.Generic;
using System.Text;

namespace PriceService.Domain.Entity
{
    public class ArticlePrice
    {
        public int ArticleId { get; set; }

        public decimal BasicPriceInEuros { get; set; }

        //TODO add unit as well, imported from ArticleService 
        //TODO add unit tests for this later
        public decimal CalculateTotalPrice(int quantityInKg, int stockInKg)
        {
            var baseTotal = BasicPriceInEuros * quantityInKg;

            var totalDiscount = CalculateDiscount(quantityInKg, stockInKg);

            var finalPrice = baseTotal * (1 - totalDiscount);

            return Math.Round(finalPrice, 2);
        }

        // TODO check on enum Eenheid as well + add unit tests for this
        private decimal CalculateDiscount(int quantityInKg, int stockInKg)
        {
            decimal totalDiscount = 0m;

            // volume discount (aka 'staffel korting' )
            if (quantityInKg >= 20)
            {
                totalDiscount += 0.20m;
            }
            else if (quantityInKg >= 10)
            {
                totalDiscount += 0.10m;
            }

            // Additional Stock discount
            if (stockInKg >= 100)
            {
                totalDiscount += 0.10m;
            }

            return totalDiscount;
        }

    }
}
