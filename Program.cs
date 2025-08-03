using Serilog.Formatting.Compact;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.DurableHttpUsingFileSizeRolledBuffers(
        requestUri: "http://localhost:8080", // logstash endpoint
        textFormatter: new RenderedCompactJsonFormatter()
    )
    .CreateLogger();

try
{
    Log.Information("Uygulama başlatıldı.");

    // Siparişler (doğru ve hatalı)
    ProcessOrder("Umut Kaan", 5);
    ProcessOrder("Ali Veli", 0);         // Hatalı
    ProcessOrder("Ayşe", -2);            // Hatalı
    ProcessOrder("Murat", 3);
    ProcessOrder("Fatma", 10);
    ProcessOrder("Zeynep", 0);           // Hatalı
    ProcessOrder("Mehmet", -1);          // Hatalı
    ProcessOrder("Ahmet", 2);
    ProcessOrder("Elif", 8);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Beklenmeyen kritik hata.");
}
finally
{
    Log.Information("Uygulama kapanıyor.");
    Log.CloseAndFlush();
}


static void ProcessOrder(string customer, int quantity)
{
    Log.Debug("ProcessOrder çağrıldı: {Customer} - {Quantity}", customer, quantity);

    if (quantity <= 0)
    {
        Log.Warning("Sipariş miktarı sıfır veya negatif olarak alındı. Müşteri: {Customer}, Miktar: {Quantity}", customer, quantity);
        throw new ArgumentException("Miktar 0'dan büyük olmalıdır.");
    }

    // Sipariş başarılı
    Log.Information("Sipariş başarıyla işlendi. Müşteri: {Customer}, Miktar: {Quantity}", customer, quantity);

    var orderId = Guid.NewGuid();
    Log.Debug("Sipariş ID oluşturuldu: {OrderId} için müşteri: {Customer}", orderId, customer);

    try
    {
        CompletePayment(orderId, customer);
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Ödeme tamamlanamadı. Sipariş ID: {OrderId}, Müşteri: {Customer}", orderId, customer);
        throw;
    }
}

static void CompletePayment(Guid orderId, string customer)
{
    Log.Debug("Ödeme işlemi başlıyor. Sipariş ID: {OrderId}, Müşteri: {Customer}", orderId, customer);

    // Her müşteri için rastgele hata simülasyonu
    var rnd = new Random();
    bool paymentSuccess = rnd.Next(0, 3) != 0; // %66 olasılıkla başarılı

    if (!paymentSuccess)
    {
        throw new InvalidOperationException("Ödeme servisine bağlanılamadı.");
    }

    Log.Information("Ödeme işlemi başarılı. Sipariş ID: {OrderId}, Müşteri: {Customer}", orderId, customer);
}
