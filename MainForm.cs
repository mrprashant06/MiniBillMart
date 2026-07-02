using MiniBillMart.Models;
using MiniBillMart.Services;

namespace MiniBillMart;

public sealed class MainForm : Form
{
    private readonly SampleDataService _dataService = new();
    private readonly List<InvoiceLine> _invoiceLines = new();

    private IReadOnlyList<Party> _parties = Array.Empty<Party>();
    private List<MonthlyPurchase> _monthlyItems = new();

    private ComboBox _partyCombo = null!;
    private Label _partyInfoLabel = null!;
    private Label _liveStatusLabel = null!;
    private DataGridView _invoiceGrid = null!;
    private DataGridView _monthlyGrid = null!;
    private Label _previewTitleLabel = null!;
    private Label _previewDetailsLabel = null!;
    private Label _summaryLabel = null!;
    private Panel _previewIconPanel = null!;

    public MainForm()
    {
        Text = "Mini Bill Mart - Grocery Billing";
        StartPosition = FormStartPosition.CenterScreen;
        Width = 1220;
        Height = 760;
        MinimumSize = new Size(1100, 680);
        BackColor = Color.FromArgb(185, 182, 245);

        BuildInterface();
        Load += (_, _) => LoadParties();
    }

    private void BuildInterface()
    {
        Controls.Add(BuildTopBar());
        Controls.Add(BuildPartyPanel());
        Controls.Add(BuildInvoicePanel());
        Controls.Add(BuildMonthlyPanel());
        Controls.Add(BuildCheckPanel());
    }

    private Control BuildTopBar()
    {
        var panel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 52,
            BackColor = Color.WhiteSmoke
        };

        panel.Controls.Add(new Label
        {
            Text = "Mini Bill Mart",
            Font = new Font("Segoe UI", 12, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(14, 8)
        });

        panel.Controls.Add(new Label
        {
            Text = "Sale Invoice | Monthly Grocery Reorder Live | Delhi NCR Data",
            Font = new Font("Segoe UI", 9),
            AutoSize = true,
            Location = new Point(14, 30)
        });

        return panel;
    }

    private Control BuildPartyPanel()
    {
        var panel = CreateBoxPanel(new Point(14, 64), new Size(1174, 92), "Party Information");

        panel.Controls.Add(new Label
        {
            Text = "Billing Party",
            AutoSize = true,
            Location = new Point(12, 28)
        });

        _partyCombo = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Location = new Point(100, 24),
            Width = 260
        };
        _partyCombo.SelectedIndexChanged += (_, _) => LoadSelectedParty();
        panel.Controls.Add(_partyCombo);

        var loadButton = CreateActionButton("Load Party", new Point(374, 22), new Size(95, 28));
        loadButton.Click += (_, _) => LoadSelectedParty();
        panel.Controls.Add(loadButton);

        _liveStatusLabel = new Label
        {
            Text = "LIVE: waiting for party selection",
            BackColor = Color.FromArgb(255, 255, 194),
            BorderStyle = BorderStyle.FixedSingle,
            Location = new Point(488, 22),
            Size = new Size(300, 28),
            TextAlign = ContentAlignment.MiddleLeft
        };
        panel.Controls.Add(_liveStatusLabel);

        _partyInfoLabel = new Label
        {
            Text = "",
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            Location = new Point(12, 58),
            Size = new Size(1138, 24),
            TextAlign = ContentAlignment.MiddleLeft
        };
        panel.Controls.Add(_partyInfoLabel);

        return panel;
    }

    private Control BuildInvoicePanel()
    {
        var panel = CreateBoxPanel(new Point(14, 168), new Size(1174, 286), "Sale Invoice - Current Bill");

        _invoiceGrid = CreateGrid(new Point(12, 28), new Size(1138, 210));
        _invoiceGrid.Columns.Add("ItemCode", "ITEM CODE");
        _invoiceGrid.Columns.Add("ProductName", "PARTICULARS");
        _invoiceGrid.Columns.Add("Pack", "PACK");
        _invoiceGrid.Columns.Add("Qty", "QTY");
        _invoiceGrid.Columns.Add("Rate", "RATE");
        _invoiceGrid.Columns.Add("Gst", "GST%");
        _invoiceGrid.Columns.Add("Amount", "AMOUNT");
        _invoiceGrid.Columns["ProductName"]!.Width = 420;
        panel.Controls.Add(_invoiceGrid);

        _summaryLabel = new Label
        {
            Text = "Items: 0 | Net Amount: 0.00",
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            BackColor = Color.FromArgb(255, 224, 189),
            BorderStyle = BorderStyle.FixedSingle,
            Location = new Point(760, 246),
            Size = new Size(390, 28),
            TextAlign = ContentAlignment.MiddleRight
        };
        panel.Controls.Add(_summaryLabel);

        var clearButton = CreateGrayButton("Clear Bill", new Point(12, 246), new Size(100, 28));
        clearButton.Click += (_, _) => ClearBill();
        panel.Controls.Add(clearButton);

        return panel;
    }

    private Control BuildMonthlyPanel()
    {
        var panel = CreateBoxPanel(new Point(14, 466), new Size(756, 226), "Mini Bill Mart Reorder Live - Grocery Suggestions");

        _monthlyGrid = CreateGrid(new Point(12, 28), new Size(728, 148));
        _monthlyGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _monthlyGrid.MultiSelect = false;
        _monthlyGrid.Columns.Add("Due", "DUE");
        _monthlyGrid.Columns.Add("ProductName", "OLD ITEM NAME");
        _monthlyGrid.Columns.Add("LastBill", "LAST BILL");
        _monthlyGrid.Columns.Add("LastQty", "LAST QTY");
        _monthlyGrid.Columns.Add("SuggestedQty", "SUGG QTY");
        _monthlyGrid.Columns.Add("Rate", "CUR RATE");
        _monthlyGrid.Columns.Add("Reason", "WHY SHOWING");
        _monthlyGrid.Columns.Add("Action", "ACTION");
        _monthlyGrid.Columns["ProductName"]!.Width = 220;
        _monthlyGrid.Columns["Reason"]!.Width = 120;
        _monthlyGrid.SelectionChanged += (_, _) => ShowSelectedMonthlyItem();
        panel.Controls.Add(_monthlyGrid);

        var addButton = CreateActionButton("Add Selected", new Point(12, 184), new Size(116, 28));
        addButton.Click += (_, _) => AddSelectedMonthlyItem();
        panel.Controls.Add(addButton);

        var skipButton = CreateGrayButton("Skip This Month", new Point(138, 184), new Size(130, 28));
        skipButton.Click += (_, _) => SkipSelectedMonthlyItem();
        panel.Controls.Add(skipButton);

        var addAllButton = CreateActionButton("Add All Due", new Point(278, 184), new Size(110, 28));
        addAllButton.Click += (_, _) => AddAllDueItems();
        panel.Controls.Add(addAllButton);

        panel.Controls.Add(new Label
        {
            Text = "Purpose: monthly grocery suggestions appear live during Sale Invoice. Operator can add, ask, or skip before final bill.",
            Location = new Point(402, 184),
            Size = new Size(338, 34),
            ForeColor = Color.FromArgb(120, 60, 0)
        });

        return panel;
    }

    private Control BuildCheckPanel()
    {
        var panel = CreateBoxPanel(new Point(782, 466), new Size(406, 226), "Monthly Billing Check - Before Save");

        _previewIconPanel = new Panel
        {
            Location = new Point(14, 34),
            Size = new Size(86, 86),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };
        _previewIconPanel.Paint += (_, e) => DrawPreviewIcon(e.Graphics);
        panel.Controls.Add(_previewIconPanel);

        _previewTitleLabel = new Label
        {
            Text = "No item selected",
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            Location = new Point(112, 34),
            Size = new Size(270, 24)
        };
        panel.Controls.Add(_previewTitleLabel);

        _previewDetailsLabel = new Label
        {
            Text = "Select party and item to see old monthly product details.",
            Location = new Point(112, 62),
            Size = new Size(270, 82)
        };
        panel.Controls.Add(_previewDetailsLabel);

        var addSelected = CreateActionButton("Add Selected", new Point(112, 154), new Size(118, 28));
        addSelected.Click += (_, _) => AddSelectedMonthlyItem();
        panel.Controls.Add(addSelected);

        var addAll = CreateActionButton("Add All Due", new Point(242, 154), new Size(118, 28));
        addAll.Click += (_, _) => AddAllDueItems();
        panel.Controls.Add(addAll);

        var oldBills = CreateGrayButton("Old Bills", new Point(112, 188), new Size(248, 28));
        oldBills.Click += (_, _) => MessageBox.Show(
            "In Mini Bill Mart, this button would open the selected party's old bills and grocery lifting history.",
            "Old Bills",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
        panel.Controls.Add(oldBills);

        return panel;
    }

    private void LoadParties()
    {
        _parties = _dataService.GetParties();
        _partyCombo.DataSource = _parties;
    }

    private void LoadSelectedParty()
    {
        if (_partyCombo.SelectedItem is not Party party)
        {
            return;
        }

        _monthlyItems = _dataService.GetMonthlyPurchases(party.Code).ToList();
        _invoiceLines.Clear();

        _partyInfoLabel.Text = $"Party: {party.Name} | Code: {party.Code} | State: {party.State} | Address: {party.Address}";
        _liveStatusLabel.Text = $"LIVE: monthly buying pattern found for {party.Name}";

        RefreshMonthlyGrid();
        RefreshInvoiceGrid();
        ShowSelectedMonthlyItem();
    }

    private void RefreshMonthlyGrid()
    {
        _monthlyGrid.Rows.Clear();

        foreach (var item in _monthlyItems)
        {
            var dueText = item.AddedToBill ? "ADDED" : item.SkippedThisMonth ? "SKIP" : "DUE";
            var actionText = item.AddedToBill ? "Already Added" : item.SkippedThisMonth ? "Skipped" : $"Add Qty {item.SuggestedQty}";
            var rowIndex = _monthlyGrid.Rows.Add(
                dueText,
                item.ProductName,
                item.LastBillDate.ToString("dd MMM"),
                item.LastQty,
                item.SuggestedQty,
                item.CurrentRate.ToString("0.00"),
                item.Reason,
                actionText);

            var row = _monthlyGrid.Rows[rowIndex];
            row.Tag = item;
            row.DefaultCellStyle.BackColor = item.AddedToBill
                ? Color.FromArgb(220, 252, 231)
                : item.SkippedThisMonth
                    ? Color.FromArgb(243, 244, 246)
                    : item.Availability == "Limited"
                        ? Color.FromArgb(255, 240, 168)
                        : Color.White;
        }
    }

    private void RefreshInvoiceGrid()
    {
        _invoiceGrid.Rows.Clear();

        foreach (var line in _invoiceLines)
        {
            _invoiceGrid.Rows.Add(
                line.ItemCode,
                line.ProductName,
                line.Pack,
                line.Qty,
                line.Rate.ToString("0.00"),
                line.GstPercent.ToString("0.##"),
                line.Amount.ToString("0.00"));
        }

        var netAmount = _invoiceLines.Sum(line => line.Amount);
        _summaryLabel.Text = $"Items: {_invoiceLines.Count} | Net Amount: {netAmount:0.00}";
    }

    private void ShowSelectedMonthlyItem()
    {
        var item = GetSelectedMonthlyItem();

        if (item is null)
        {
            _previewTitleLabel.Text = "No item selected";
            _previewDetailsLabel.Text = "Select a monthly old item to see details.";
            _previewIconPanel.Invalidate();
            return;
        }

        _previewTitleLabel.Text = item.ProductName;
        _previewDetailsLabel.Text =
            $"Pack: {item.Pack} | Last Qty: {item.LastQty} | Suggested Qty: {item.SuggestedQty}{Environment.NewLine}" +
            $"Last Bill: {item.LastBillDate:dd MMM yyyy} | Rate: {item.CurrentRate:0.00}{Environment.NewLine}" +
            $"MRP: {item.Mrp:0.00} | GST: {item.GstPercent:0.##}% | Status: {item.Availability}{Environment.NewLine}" +
            $"Reason: {item.Reason}";
        _previewIconPanel.Invalidate();
    }

    private void AddSelectedMonthlyItem()
    {
        var item = GetSelectedMonthlyItem();
        if (item is null || item.AddedToBill)
        {
            return;
        }

        AddMonthlyItemToInvoice(item);
        item.AddedToBill = true;
        item.SkippedThisMonth = false;
        RefreshMonthlyGrid();
        RefreshInvoiceGrid();
    }

    private void AddAllDueItems()
    {
        foreach (var item in _monthlyItems.Where(item => !item.AddedToBill && !item.SkippedThisMonth))
        {
            AddMonthlyItemToInvoice(item);
            item.AddedToBill = true;
        }

        RefreshMonthlyGrid();
        RefreshInvoiceGrid();
    }

    private void SkipSelectedMonthlyItem()
    {
        var item = GetSelectedMonthlyItem();
        if (item is null || item.AddedToBill)
        {
            return;
        }

        item.SkippedThisMonth = true;
        RefreshMonthlyGrid();
        ShowSelectedMonthlyItem();
    }

    private void ClearBill()
    {
        _invoiceLines.Clear();
        foreach (var item in _monthlyItems)
        {
            item.AddedToBill = false;
            item.SkippedThisMonth = false;
        }

        RefreshMonthlyGrid();
        RefreshInvoiceGrid();
        ShowSelectedMonthlyItem();
    }

    private MonthlyPurchase? GetSelectedMonthlyItem()
    {
        if (_monthlyGrid.CurrentRow?.Tag is MonthlyPurchase item)
        {
            return item;
        }

        return null;
    }

    private void AddMonthlyItemToInvoice(MonthlyPurchase item)
    {
        var existingLine = _invoiceLines.FirstOrDefault(line => line.ItemCode == item.ItemCode);
        if (existingLine is not null)
        {
            existingLine.AddQty(item.SuggestedQty);
            return;
        }

        _invoiceLines.Add(new InvoiceLine(
            item.ItemCode,
            item.ProductName,
            item.Pack,
            item.SuggestedQty,
            item.CurrentRate,
            item.GstPercent));
    }

    private void DrawPreviewIcon(Graphics graphics)
    {
        graphics.Clear(Color.White);

        var item = GetSelectedMonthlyItem();
        var productInitial = item?.ProductName.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "ITEM";
        var label = productInitial.Length > 3 ? productInitial[..3] : productInitial;

        using var bodyBrush = new SolidBrush(Color.FromArgb(120, 217, 139));
        using var borderPen = new Pen(Color.DimGray);
        using var labelBrush = new SolidBrush(Color.White);
        using var textBrush = new SolidBrush(Color.Black);
        using var font = new Font("Segoe UI", 8, FontStyle.Bold);

        graphics.FillRectangle(bodyBrush, 28, 18, 30, 44);
        graphics.DrawRectangle(borderPen, 28, 18, 30, 44);
        graphics.FillRectangle(bodyBrush, 36, 12, 14, 8);
        graphics.DrawRectangle(borderPen, 36, 12, 14, 8);
        graphics.FillRectangle(labelBrush, 32, 32, 22, 14);
        graphics.DrawRectangle(borderPen, 32, 32, 22, 14);
        graphics.DrawString(label, font, textBrush, 33, 31);
    }

    private static Panel CreateBoxPanel(Point location, Size size, string title)
    {
        var panel = new Panel
        {
            Location = location,
            Size = size,
            BackColor = Color.FromArgb(255, 247, 199),
            BorderStyle = BorderStyle.FixedSingle
        };

        var header = new Label
        {
            Text = title,
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            BackColor = Color.FromArgb(255, 226, 176),
            Location = new Point(0, 0),
            Size = new Size(size.Width - 2, 22),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(8, 0, 0, 0)
        };
        panel.Controls.Add(header);

        return panel;
    }

    private static DataGridView CreateGrid(Point location, Size size)
    {
        return new DataGridView
        {
            Location = location,
            Size = size,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            ReadOnly = true,
            RowHeadersVisible = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            EnableHeadersVisualStyles = false,
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
            ColumnHeadersHeight = 26,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };
    }

    private static Button CreateActionButton(string text, Point location, Size size)
    {
        return new Button
        {
            Text = text,
            Location = location,
            Size = size,
            BackColor = Color.FromArgb(0, 255, 38),
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9, FontStyle.Bold)
        };
    }

    private static Button CreateGrayButton(string text, Point location, Size size)
    {
        return new Button
        {
            Text = text,
            Location = location,
            Size = size,
            BackColor = Color.FromArgb(231, 231, 231),
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 9, FontStyle.Bold)
        };
    }
}
