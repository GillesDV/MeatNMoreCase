using System;
using System.Collections.Generic;
using System.Text;

namespace PriceService.Domain.Entity
{
    public class ArticlePrice
    {
        public int ArticleId { get; set; }

        public decimal BasicPriceInEuros { get; set; }

        public decimal CalculateTotalPrice(int quantityInKg, int stockInKg)
        {
            var baseTotal = BasicPriceInEuros * quantityInKg;

            var totalDiscount = GetTierDiscount(quantityInKg);

            if (stockInKg >= 100)
            {
                totalDiscount += 0.10m;
            }

            var finalPrice = baseTotal * (1 - totalDiscount);

            return Math.Round(finalPrice, 2);
        }

        // TODO check on enum Eenheid as well + add unit tests for this
        private decimal GetTierDiscount(int quantityInKg)
        {
            // You can do this in other ways (switch case, Linq / ordering...), but this is straight forward & simply-to-read by design for now.
            if (quantityInKg >= 20)
            {
                return 0.20m;
            }

            if (quantityInKg >= 10)
            {
                return 0.10m;
            }

            return 0m;
        }

    }
}
