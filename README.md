# Mini Bill Mart.....

This is a small C# Windows Forms grocery billing project for supermarket monthly reorder suggestions.

The goal is to show how Mini Bill Mart can display party-wise monthly grocery suggestions while a bill is being prepared.

## What Mini Bill Mart Shows...........

- Select a billing party/customer.
- Load that party's regular monthly grocery products from sample offline data.
- Show old monthly grocery products.
- Add selected suggested quantity into the invoice grid.
- Add all due monthly items together.
- Skip an item for this month.
- Show a simple live item preview.
- Keep customer-safe logic: no purchase rate, no margin, no supplier data.

## Technology...............

- C#
- .NET 9
- Windows Forms
- Offline in-memory Delhi/Gurugram grocery sample data

## How To Run.........

Open PowerShell in this folder and run:

```powershell
dotnet run
```

Or build only:

```powershell
dotnet build
```

## Main Learning Files.......

`Program.cs`

Starts the Windows Forms app.

`MainForm.cs`

Creates the UI and handles button clicks:

- party selection
- monthly item loading
- add selected item
- add all due items
- skip item
- invoice total calculation

`Models/Party.cs`

Represents a billing party/customer.

`Models/MonthlyPurchase.cs`

Represents an old monthly product bought by a party.

`Models/InvoiceLine.cs`

Represents one line in the current invoice.

`Services/SampleDataService.cs`

Provides sample party and monthly grocery product data. In a real Mini Bill Mart app, this would come from the local database.

## Mini Bill Mart Feature Idea

When the operator selects a regular billing party, the software should check old invoices and show products that the party usually buys every month.

Recommended interface name:

`Mini Bill Mart Reorder Live`

Suggested table columns:

- Due
- Old Item Name
- Last Bill
- Last Qty
- Suggested Qty
- Current Rate
- Why Showing
- Action

## Real Mini Bill Mart Database Logic

In a real app, replace `SampleDataService` with database queries:

- party master
- item master
- sale invoice header
- sale invoice details
- stock table
- batch table
- GST/rate table

The feature should use current grocery item rate, GST, batch, stock and scheme when adding the item to the invoice.

## GitHub Note

This folder is ready to push to GitHub. Git is not required to run the app, but you need Git installed to push it.

Typical commands after installing Git:

```powershell
git init
git add .
git commit -m "Add Mini Bill Mart grocery billing app"
git branch -M main
git remote add origin https://github.com/YOUR-USER/YOUR-REPO.git
git push -u origin main
```
