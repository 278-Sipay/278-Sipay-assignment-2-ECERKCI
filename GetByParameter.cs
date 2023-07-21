using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;

// Diyelim ki TransactionController içerisinde Transaction modelimiz var.
// Aşağıdaki gibi örneğin AccountNumber, AmountCredit, AmountDebit, Description, BeginDate, EndDate ve ReferenceNumber alanlarına sahip.

public class Transaction
{
    public int AccountNumber { get; set; }
    public decimal AmountCredit { get; set; }
    public decimal AmountDebit { get; set; }
    public string Description { get; set; }
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
    public int ReferenceNumber { get; set; }
    // Diğer özellikler...
}

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionController(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    [HttpGet("GetByParameter")]
    public IActionResult GetByParameter(
        int? accountNumber,
        decimal? minAmountCredit,
        decimal? maxAmountCredit,
        decimal? minAmountDebit,
        decimal? maxAmountDebit,
        string description,
        DateTime? beginDate,
        DateTime? endDate,
        int? referenceNumber
    )
    {
        // Bu API, istenilen kriterlere göre filtreleme yapacak.
        // Gelen kriterlere göre bir Expression oluşturup repository'de ilgili fonksiyonu çağırmamız gerekecek.

        Expression<Func<Transaction, bool>> filter = x =>
            (!accountNumber.HasValue || x.AccountNumber == accountNumber.Value)
            && (!minAmountCredit.HasValue || x.AmountCredit >= minAmountCredit.Value)
            && (!maxAmountCredit.HasValue || x.AmountCredit <= maxAmountCredit.Value)
            && (!minAmountDebit.HasValue || x.AmountDebit >= minAmountDebit.Value)
            && (!maxAmountDebit.HasValue || x.AmountDebit <= maxAmountDebit.Value)
            && (string.IsNullOrEmpty(description) || x.Description.Contains(description))
            && (!beginDate.HasValue || x.BeginDate >= beginDate.Value)
            && (!endDate.HasValue || x.EndDate <= endDate.Value)
            && (!referenceNumber.HasValue || x.ReferenceNumber == referenceNumber.Value);

        var result = _transactionRepository.GetByFilter(filter);
        return Ok(result);
    }
}
