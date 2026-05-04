using System;
using System.Collections.Generic;
using System.Text;

namespace PriceService.Domain.Entity
{
    public class ArticlePrice
    {
        public int ArticleId { get; set; }

        public decimal BasicPriceInEuros { get; set; }

        //TODO add unit tests for this later
        public decimal CalculateTotalPrice(int quantityInKg, int quantityInStock, string unit)
        {
            var baseTotal = BasicPriceInEuros * quantityInKg;

            var totalDiscount = CalculateDiscount(quantityInKg, quantityInStock, unit);

            var finalPrice = baseTotal * (1 - totalDiscount);

            return Math.Round(finalPrice, 2);
        }

        // TODO check on enum Eenheid as well + add unit tests for this
        private decimal CalculateDiscount(int quantityInKg, int quantityInStock, string unit)
        {
            decimal totalDiscount = 0m;

            // volume discount (aka 'staffel korting' )
            if (quantityInKg >= 20 && unit.Equals("kilogram", StringComparison.InvariantCultureIgnoreCase) )
            {
                totalDiscount += 0.20m;
            }
            else if (quantityInKg >= 10 && unit.Equals("kilogram", StringComparison.InvariantCultureIgnoreCase))
            {
                totalDiscount += 0.10m;
            }

            // Additional Stock discount
            if (quantityInStock >= 100 && unit.Equals("kilogram", StringComparison.InvariantCultureIgnoreCase))
            {
                totalDiscount += 0.10m;
            }

            return totalDiscount;
        }

    }
}
