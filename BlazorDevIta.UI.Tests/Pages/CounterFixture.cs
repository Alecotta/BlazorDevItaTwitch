namespace BlazorDevIta.UI.Tests.Pages;

//Questo attributo permette di dire che questa classe contiene un insieme di test e li mostra nel text explorer.
//Ogni metodo definisce una feature che si va a testare e deve descrivere il risultato che si vuole ottenere.
//Per effettuare i test bisogna aggiungere il pacchetto bunit.web.
[TestFixture]
public class CounterFixture
{
    [Test]
    public void FirstView_ShouldViewCounterWithZero()
    {
        //Si recupera il contesto di BUnit. Mette a disposizione un ambiente che si integra con il framework Blazor
        //e permette di interagire con un componente (avendo a disposizione sia il markup che il DOM generato).
        var ctx = new Bunit.TestContext();

        //Viene renderizzato un particolare componente.
        var counter = ctx.RenderComponent<Counter>();

        //Viene confrontato il markup (dopo la renderizzazione).
        counter.MarkupMatches(@"<h1>Counter</h1>
            <p role=""status"">Current count: 0</p>
            <button class=""btn btn-primary"">Click me</button>");
    }

    [Test]
    public void ClickButton_ShouldViewCounterWithOne()
    {
        var ctx = new Bunit.TestContext();

        var counter = ctx.RenderComponent<Counter>();

        //Si può recuperare un elemento tramite un selettore CSS.
        var button = counter.Find(".btn-primary");
        //Simulato il click sul bottone.
        button.Click();

        //Viene confrontato il markup (dopo la renderizzazione).
        counter.MarkupMatches(@"<h1>Counter</h1>
            <p role=""status"">Current count: 1</p>
            <button class=""btn btn-primary"">Click me</button>");
    }
}
