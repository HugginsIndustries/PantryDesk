namespace PantryDeskApp.Forms;

partial class StatsForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        grpDateRange = new GroupBox();
        lblDateRange = new Label();
        cmbDateRange = new ComboBox();
        lblStartDate = new Label();
        dtpStartDate = new DateTimePicker();
        lblEndDate = new Label();
        dtpEndDate = new DateTimePicker();
        btnPageDemographics = new Button();
        btnPageServices = new Button();
        pnlSummaryCards = new Panel();
        pnlCardTotalActiveHouseholds = new Panel();
        lblCardTotalActiveHouseholds = new Label();
        lblCardTotalActiveHouseholdsValue = new Label();
        pnlCardTotalPeople = new Panel();
        lblCardTotalPeople = new Label();
        lblCardTotalPeopleValue = new Label();
        pnlCardCompletedServices = new Panel();
        lblCardCompletedServices = new Label();
        lblCardCompletedServicesValue = new Label();
        pnlCardUniqueHouseholdsServed = new Panel();
        lblCardUniqueHouseholdsServed = new Label();
        lblCardUniqueHouseholdsServedValue = new Label();
        pnlDemographics = new Panel();
        tableLayoutDemographics = new TableLayoutPanel();
        pnlChartRaceDistribution = new Panel();
        pnlChartVeteranDistribution = new Panel();
        pnlChartDisabilityDistribution = new Panel();
        plotViewRaceDistribution = new OxyPlot.WindowsForms.PlotView();
        plotViewVeteranDistribution = new OxyPlot.WindowsForms.PlotView();
        plotViewDisabilityDistribution = new OxyPlot.WindowsForms.PlotView();
        pnlServices = new Panel();
        tableLayoutServicesRow1 = new TableLayoutPanel();
        pnlChartVisitType = new Panel();
        pnlChartEventType = new Panel();
        plotViewVisitType = new OxyPlot.WindowsForms.PlotView();
        plotViewEventType = new OxyPlot.WindowsForms.PlotView();
        pnlChartCityDistribution = new Panel();
        plotViewCityDistribution = new OxyPlot.WindowsForms.PlotView();
        pnlChartAgeGroupDistribution = new Panel();
        plotViewAgeGroupDistribution = new OxyPlot.WindowsForms.PlotView();
        pnlChartMonthlyVisitsTrend = new Panel();
        plotViewMonthlyVisitsTrend = new OxyPlot.WindowsForms.PlotView();
        pnlChartPantryDayVolume = new Panel();
        plotViewPantryDayVolume = new OxyPlot.WindowsForms.PlotView();
        btnExportPdf = new Button();
        btnPrint = new Button();
        grpDateRange.SuspendLayout();
        pnlSummaryCards.SuspendLayout();
        pnlCardTotalActiveHouseholds.SuspendLayout();
        pnlCardTotalPeople.SuspendLayout();
        pnlCardCompletedServices.SuspendLayout();
        pnlCardUniqueHouseholdsServed.SuspendLayout();
        pnlDemographics.SuspendLayout();
        tableLayoutDemographics.SuspendLayout();
        pnlChartCityDistribution.SuspendLayout();
        pnlChartAgeGroupDistribution.SuspendLayout();
        pnlChartRaceDistribution.SuspendLayout();
        pnlChartVeteranDistribution.SuspendLayout();
        pnlChartDisabilityDistribution.SuspendLayout();
        pnlServices.SuspendLayout();
        tableLayoutServicesRow1.SuspendLayout();
        pnlChartVisitType.SuspendLayout();
        pnlChartEventType.SuspendLayout();
        pnlChartMonthlyVisitsTrend.SuspendLayout();
        pnlChartPantryDayVolume.SuspendLayout();
        SuspendLayout();
        // 
        // grpDateRange
        // 
        grpDateRange.Controls.Add(btnPrint);
        grpDateRange.Controls.Add(btnExportPdf);
        grpDateRange.Controls.Add(btnPageDemographics);
        grpDateRange.Controls.Add(btnPageServices);
        grpDateRange.Controls.Add(lblDateRange);
        grpDateRange.Controls.Add(cmbDateRange);
        grpDateRange.Controls.Add(lblStartDate);
        grpDateRange.Controls.Add(dtpStartDate);
        grpDateRange.Controls.Add(lblEndDate);
        grpDateRange.Controls.Add(dtpEndDate);
        grpDateRange.Location = new Point(12, 12);
        grpDateRange.Name = "grpDateRange";
        grpDateRange.Size = new Size(1600, 60);
        grpDateRange.TabIndex = 0;
        grpDateRange.TabStop = false;
        grpDateRange.Text = "Year";
        grpDateRange.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        // 
        // lblDateRange
        // 
        lblDateRange.AutoSize = true;
        lblDateRange.Location = new Point(12, 25);
        lblDateRange.Name = "lblDateRange";
        lblDateRange.Size = new Size(68, 15);
        lblDateRange.TabIndex = 0;
        lblDateRange.Text = "Year:";
        // 
        // cmbDateRange
        // 
        cmbDateRange.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbDateRange.FormattingEnabled = true;
        cmbDateRange.Location = new Point(86, 22);
        cmbDateRange.Name = "cmbDateRange";
        cmbDateRange.Size = new Size(200, 23);
        cmbDateRange.TabIndex = 1;
        cmbDateRange.SelectedIndexChanged += CmbDateRange_SelectedIndexChanged;
        // 
        // lblStartDate
        // 
        lblStartDate.AutoSize = true;
        lblStartDate.Location = new Point(500, 25);
        lblStartDate.Name = "lblStartDate";
        lblStartDate.Size = new Size(61, 15);
        lblStartDate.TabIndex = 2;
        lblStartDate.Text = "Start Date:";
        lblStartDate.Visible = false;
        // 
        // dtpStartDate
        // 
        dtpStartDate.Location = new Point(567, 22);
        dtpStartDate.Name = "dtpStartDate";
        dtpStartDate.Size = new Size(200, 23);
        dtpStartDate.TabIndex = 3;
        dtpStartDate.Visible = false;
        // 
        // lblEndDate
        // 
        lblEndDate.AutoSize = true;
        lblEndDate.Location = new Point(780, 25);
        lblEndDate.Name = "lblEndDate";
        lblEndDate.Size = new Size(57, 15);
        lblEndDate.TabIndex = 4;
        lblEndDate.Text = "End Date:";
        lblEndDate.Visible = false;
        // 
        // dtpEndDate
        // 
        dtpEndDate.Location = new Point(843, 22);
        dtpEndDate.Name = "dtpEndDate";
        dtpEndDate.Size = new Size(200, 23);
        dtpEndDate.TabIndex = 5;
        dtpEndDate.Visible = false;
        // 
        // btnPageDemographics
        // 
        btnPageDemographics.Location = new Point(296, 22);
        btnPageDemographics.Name = "btnPageDemographics";
        btnPageDemographics.Size = new Size(100, 23);
        btnPageDemographics.TabIndex = 8;
        btnPageDemographics.Text = "Demographics";
        btnPageDemographics.UseVisualStyleBackColor = true;
        btnPageDemographics.Click += BtnPageDemographics_Click;
        btnPageDemographics.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        // 
        // btnPageServices
        // 
        btnPageServices.Location = new Point(402, 22);
        btnPageServices.Name = "btnPageServices";
        btnPageServices.Size = new Size(80, 23);
        btnPageServices.TabIndex = 9;
        btnPageServices.Text = "Services";
        btnPageServices.UseVisualStyleBackColor = true;
        btnPageServices.Click += BtnPageServices_Click;
        btnPageServices.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        // 
        // pnlSummaryCards
        // 
        pnlSummaryCards.Controls.Add(pnlCardTotalActiveHouseholds);
        pnlSummaryCards.Controls.Add(pnlCardTotalPeople);
        pnlSummaryCards.Controls.Add(pnlCardCompletedServices);
        pnlSummaryCards.Controls.Add(pnlCardUniqueHouseholdsServed);
        pnlSummaryCards.Location = new Point(12, 78);
        pnlSummaryCards.Name = "pnlSummaryCards";
        pnlSummaryCards.Size = new Size(1600, 100);
        pnlSummaryCards.TabIndex = 1;
        pnlSummaryCards.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlSummaryCards.Dock = DockStyle.None;
        // 
        // pnlCardTotalActiveHouseholds
        // 
        pnlCardTotalActiveHouseholds.BorderStyle = BorderStyle.FixedSingle;
        pnlCardTotalActiveHouseholds.Controls.Add(lblCardTotalActiveHouseholds);
        pnlCardTotalActiveHouseholds.Controls.Add(lblCardTotalActiveHouseholdsValue);
        pnlCardTotalActiveHouseholds.Location = new Point(0, 0);
        pnlCardTotalActiveHouseholds.Name = "pnlCardTotalActiveHouseholds";
        pnlCardTotalActiveHouseholds.Size = new Size(390, 100);
        pnlCardTotalActiveHouseholds.TabIndex = 0;
        pnlCardTotalActiveHouseholds.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        // 
        // lblCardTotalActiveHouseholds
        // 
        lblCardTotalActiveHouseholds.AutoSize = true;
        lblCardTotalActiveHouseholds.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        lblCardTotalActiveHouseholds.Location = new Point(10, 10);
        lblCardTotalActiveHouseholds.Name = "lblCardTotalActiveHouseholds";
        lblCardTotalActiveHouseholds.Size = new Size(150, 15);
        lblCardTotalActiveHouseholds.TabIndex = 0;
        lblCardTotalActiveHouseholds.Text = "Unique Households Served";
        // 
        // lblCardTotalActiveHouseholdsValue
        // 
        lblCardTotalActiveHouseholdsValue.AutoSize = true;
        lblCardTotalActiveHouseholdsValue.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point);
        lblCardTotalActiveHouseholdsValue.Location = new Point(10, 35);
        lblCardTotalActiveHouseholdsValue.Name = "lblCardTotalActiveHouseholdsValue";
        lblCardTotalActiveHouseholdsValue.Size = new Size(38, 45);
        lblCardTotalActiveHouseholdsValue.TabIndex = 1;
        lblCardTotalActiveHouseholdsValue.Text = "0";
        // 
        // pnlCardTotalPeople
        // 
        pnlCardTotalPeople.BorderStyle = BorderStyle.FixedSingle;
        pnlCardTotalPeople.Controls.Add(lblCardTotalPeople);
        pnlCardTotalPeople.Controls.Add(lblCardTotalPeopleValue);
        pnlCardTotalPeople.Location = new Point(400, 0);
        pnlCardTotalPeople.Name = "pnlCardTotalPeople";
        pnlCardTotalPeople.Size = new Size(390, 100);
        pnlCardTotalPeople.TabIndex = 1;
        pnlCardTotalPeople.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        // 
        // lblCardTotalPeople
        // 
        lblCardTotalPeople.AutoSize = true;
        lblCardTotalPeople.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        lblCardTotalPeople.Location = new Point(10, 10);
        lblCardTotalPeople.Name = "lblCardTotalPeople";
        lblCardTotalPeople.Size = new Size(75, 15);
        lblCardTotalPeople.TabIndex = 0;
        lblCardTotalPeople.Text = "Total People";
        // 
        // lblCardTotalPeopleValue
        // 
        lblCardTotalPeopleValue.AutoSize = true;
        lblCardTotalPeopleValue.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point);
        lblCardTotalPeopleValue.Location = new Point(10, 35);
        lblCardTotalPeopleValue.Name = "lblCardTotalPeopleValue";
        lblCardTotalPeopleValue.Size = new Size(38, 45);
        lblCardTotalPeopleValue.TabIndex = 1;
        lblCardTotalPeopleValue.Text = "0";
        // 
        // pnlCardCompletedServices
        // 
        pnlCardCompletedServices.BorderStyle = BorderStyle.FixedSingle;
        pnlCardCompletedServices.Controls.Add(lblCardCompletedServices);
        pnlCardCompletedServices.Controls.Add(lblCardCompletedServicesValue);
        pnlCardCompletedServices.Location = new Point(800, 0);
        pnlCardCompletedServices.Name = "pnlCardCompletedServices";
        pnlCardCompletedServices.Size = new Size(390, 100);
        pnlCardCompletedServices.TabIndex = 2;
        pnlCardCompletedServices.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        // 
        // lblCardCompletedServices
        // 
        lblCardCompletedServices.AutoSize = true;
        lblCardCompletedServices.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        lblCardCompletedServices.Location = new Point(10, 10);
        lblCardCompletedServices.Name = "lblCardCompletedServices";
        lblCardCompletedServices.Size = new Size(120, 15);
        lblCardCompletedServices.TabIndex = 0;
        lblCardCompletedServices.Text = "Completed Services";
        // 
        // lblCardCompletedServicesValue
        // 
        lblCardCompletedServicesValue.AutoSize = true;
        lblCardCompletedServicesValue.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point);
        lblCardCompletedServicesValue.Location = new Point(10, 35);
        lblCardCompletedServicesValue.Name = "lblCardCompletedServicesValue";
        lblCardCompletedServicesValue.Size = new Size(38, 45);
        lblCardCompletedServicesValue.TabIndex = 1;
        lblCardCompletedServicesValue.Text = "0";
        // 
        // pnlCardUniqueHouseholdsServed
        // 
        pnlCardUniqueHouseholdsServed.BorderStyle = BorderStyle.FixedSingle;
        pnlCardUniqueHouseholdsServed.Controls.Add(lblCardUniqueHouseholdsServed);
        pnlCardUniqueHouseholdsServed.Controls.Add(lblCardUniqueHouseholdsServedValue);
        pnlCardUniqueHouseholdsServed.Location = new Point(1200, 0);
        pnlCardUniqueHouseholdsServed.Name = "pnlCardUniqueHouseholdsServed";
        pnlCardUniqueHouseholdsServed.Size = new Size(390, 100);
        pnlCardUniqueHouseholdsServed.TabIndex = 3;
        pnlCardUniqueHouseholdsServed.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        // 
        // lblCardUniqueHouseholdsServed
        // 
        lblCardUniqueHouseholdsServed.AutoSize = true;
        lblCardUniqueHouseholdsServed.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
        lblCardUniqueHouseholdsServed.Location = new Point(10, 10);
        lblCardUniqueHouseholdsServed.Name = "lblCardUniqueHouseholdsServed";
        lblCardUniqueHouseholdsServed.Size = new Size(180, 15);
        lblCardUniqueHouseholdsServed.TabIndex = 0;
        lblCardUniqueHouseholdsServed.Text = "Deck Total";
        // 
        // lblCardUniqueHouseholdsServedValue
        // 
        lblCardUniqueHouseholdsServedValue.AutoSize = true;
        lblCardUniqueHouseholdsServedValue.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point);
        lblCardUniqueHouseholdsServedValue.Location = new Point(10, 35);
        lblCardUniqueHouseholdsServedValue.Name = "lblCardUniqueHouseholdsServedValue";
        lblCardUniqueHouseholdsServedValue.Size = new Size(38, 45);
        lblCardUniqueHouseholdsServedValue.TabIndex = 1;
        lblCardUniqueHouseholdsServedValue.Text = "0";
        // 
        // pnlDemographics
        // 
        pnlDemographics.Controls.Add(tableLayoutDemographics);
        pnlDemographics.Location = new Point(12, 184);
        pnlDemographics.Name = "pnlDemographics";
        pnlDemographics.Size = new Size(1600, 786);
        pnlDemographics.TabIndex = 2;
        pnlDemographics.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlDemographics.Dock = DockStyle.None;
        // 
        // tableLayoutDemographics
        // Top row: 2 columns (50% each). Bottom row: 3 columns (33.33% each).
        // 6-column grid: top row spans 3+3, bottom row spans 2+2+2.
        // 
        tableLayoutDemographics.ColumnCount = 6;
        for (int i = 0; i < 6; i++)
        {
            tableLayoutDemographics.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / 6));
        }
        tableLayoutDemographics.Controls.Add(pnlChartCityDistribution, 0, 0);
        tableLayoutDemographics.SetColumnSpan(pnlChartCityDistribution, 3);
        tableLayoutDemographics.Controls.Add(pnlChartAgeGroupDistribution, 3, 0);
        tableLayoutDemographics.SetColumnSpan(pnlChartAgeGroupDistribution, 3);
        tableLayoutDemographics.Controls.Add(pnlChartRaceDistribution, 0, 1);
        tableLayoutDemographics.SetColumnSpan(pnlChartRaceDistribution, 2);
        tableLayoutDemographics.Controls.Add(pnlChartVeteranDistribution, 2, 1);
        tableLayoutDemographics.SetColumnSpan(pnlChartVeteranDistribution, 2);
        tableLayoutDemographics.Controls.Add(pnlChartDisabilityDistribution, 4, 1);
        tableLayoutDemographics.SetColumnSpan(pnlChartDisabilityDistribution, 2);
        tableLayoutDemographics.Dock = DockStyle.Fill;
        tableLayoutDemographics.Location = new Point(0, 0);
        tableLayoutDemographics.Name = "tableLayoutDemographics";
        tableLayoutDemographics.RowCount = 2;
        tableLayoutDemographics.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutDemographics.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutDemographics.Size = new Size(1600, 786);
        tableLayoutDemographics.TabIndex = 0;
        // 
        // pnlChartRaceDistribution
        // 
        pnlChartRaceDistribution.Controls.Add(plotViewRaceDistribution);
        pnlChartRaceDistribution.Dock = DockStyle.Fill;
        pnlChartRaceDistribution.Location = new Point(3, 396);
        pnlChartRaceDistribution.Name = "pnlChartRaceDistribution";
        pnlChartRaceDistribution.Size = new Size(527, 387);
        pnlChartRaceDistribution.TabIndex = 2;
        // 
        // plotViewRaceDistribution
        // 
        plotViewRaceDistribution.Dock = DockStyle.Fill;
        plotViewRaceDistribution.Location = new Point(0, 0);
        plotViewRaceDistribution.Name = "plotViewRaceDistribution";
        plotViewRaceDistribution.PanCursor = Cursors.Hand;
        plotViewRaceDistribution.Size = new Size(527, 387);
        plotViewRaceDistribution.TabIndex = 0;
        plotViewRaceDistribution.Text = "Race Distribution";
        plotViewRaceDistribution.ZoomHorizontalCursor = Cursors.SizeWE;
        plotViewRaceDistribution.ZoomRectangleCursor = Cursors.SizeNWSE;
        plotViewRaceDistribution.ZoomVerticalCursor = Cursors.SizeNS;
        // 
        // pnlChartVeteranDistribution
        // 
        pnlChartVeteranDistribution.Controls.Add(plotViewVeteranDistribution);
        pnlChartVeteranDistribution.Dock = DockStyle.Fill;
        pnlChartVeteranDistribution.Location = new Point(536, 396);
        pnlChartVeteranDistribution.Name = "pnlChartVeteranDistribution";
        pnlChartVeteranDistribution.Size = new Size(527, 387);
        pnlChartVeteranDistribution.TabIndex = 3;
        // 
        // plotViewVeteranDistribution
        // 
        plotViewVeteranDistribution.Dock = DockStyle.Fill;
        plotViewVeteranDistribution.Location = new Point(0, 0);
        plotViewVeteranDistribution.Name = "plotViewVeteranDistribution";
        plotViewVeteranDistribution.PanCursor = Cursors.Hand;
        plotViewVeteranDistribution.Size = new Size(527, 387);
        plotViewVeteranDistribution.TabIndex = 0;
        plotViewVeteranDistribution.Text = "Veteran Status Distribution";
        plotViewVeteranDistribution.ZoomHorizontalCursor = Cursors.SizeWE;
        plotViewVeteranDistribution.ZoomRectangleCursor = Cursors.SizeNWSE;
        plotViewVeteranDistribution.ZoomVerticalCursor = Cursors.SizeNS;
        // 
        // pnlChartDisabilityDistribution
        // 
        pnlChartDisabilityDistribution.Controls.Add(plotViewDisabilityDistribution);
        pnlChartDisabilityDistribution.Dock = DockStyle.Fill;
        pnlChartDisabilityDistribution.Location = new Point(1069, 396);
        pnlChartDisabilityDistribution.Name = "pnlChartDisabilityDistribution";
        pnlChartDisabilityDistribution.Size = new Size(528, 387);
        pnlChartDisabilityDistribution.TabIndex = 4;
        // 
        // plotViewDisabilityDistribution
        // 
        plotViewDisabilityDistribution.Dock = DockStyle.Fill;
        plotViewDisabilityDistribution.Location = new Point(0, 0);
        plotViewDisabilityDistribution.Name = "plotViewDisabilityDistribution";
        plotViewDisabilityDistribution.PanCursor = Cursors.Hand;
        plotViewDisabilityDistribution.Size = new Size(528, 387);
        plotViewDisabilityDistribution.TabIndex = 0;
        plotViewDisabilityDistribution.Text = "Disability Status Distribution";
        plotViewDisabilityDistribution.ZoomHorizontalCursor = Cursors.SizeWE;
        plotViewDisabilityDistribution.ZoomRectangleCursor = Cursors.SizeNWSE;
        plotViewDisabilityDistribution.ZoomVerticalCursor = Cursors.SizeNS;
        // 
        // pnlServices
        // 
        pnlServices.Controls.Add(pnlChartPantryDayVolume);
        pnlServices.Controls.Add(tableLayoutServicesRow1);
        pnlServices.Location = new Point(12, 184);
        pnlServices.Name = "pnlServices";
        pnlServices.Size = new Size(1600, 786);
        pnlServices.TabIndex = 4;
        pnlServices.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlServices.Visible = false;
        pnlServices.Dock = DockStyle.None;
        // 
        // tableLayoutServicesRow1
        // 
        tableLayoutServicesRow1.ColumnCount = 3;
        tableLayoutServicesRow1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tableLayoutServicesRow1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tableLayoutServicesRow1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutServicesRow1.Controls.Add(pnlChartVisitType, 0, 0);
        tableLayoutServicesRow1.Controls.Add(pnlChartEventType, 1, 0);
        tableLayoutServicesRow1.Controls.Add(pnlChartMonthlyVisitsTrend, 2, 0);
        tableLayoutServicesRow1.Dock = DockStyle.Top;
        tableLayoutServicesRow1.Location = new Point(0, 0);
        tableLayoutServicesRow1.Name = "tableLayoutServicesRow1";
        tableLayoutServicesRow1.RowCount = 1;
        tableLayoutServicesRow1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tableLayoutServicesRow1.Size = new Size(1600, 450);
        tableLayoutServicesRow1.TabIndex = 0;
        // 
        // pnlChartVisitType
        // 
        pnlChartVisitType.Controls.Add(plotViewVisitType);
        pnlChartVisitType.Dock = DockStyle.Fill;
        pnlChartVisitType.Location = new Point(3, 3);
        pnlChartVisitType.Name = "pnlChartVisitType";
        pnlChartVisitType.Size = new Size(392, 444);
        pnlChartVisitType.TabIndex = 0;
        // 
        // plotViewVisitType
        // 
        plotViewVisitType.Dock = DockStyle.Fill;
        plotViewVisitType.Location = new Point(0, 0);
        plotViewVisitType.Name = "plotViewVisitType";
        plotViewVisitType.PanCursor = Cursors.Hand;
        plotViewVisitType.Size = new Size(392, 444);
        plotViewVisitType.TabIndex = 0;
        plotViewVisitType.Text = "Visit Type";
        plotViewVisitType.ZoomHorizontalCursor = Cursors.SizeWE;
        plotViewVisitType.ZoomRectangleCursor = Cursors.SizeNWSE;
        plotViewVisitType.ZoomVerticalCursor = Cursors.SizeNS;
        // 
        // pnlChartEventType
        // 
        pnlChartEventType.Controls.Add(plotViewEventType);
        pnlChartEventType.Dock = DockStyle.Fill;
        pnlChartEventType.Location = new Point(401, 3);
        pnlChartEventType.Name = "pnlChartEventType";
        pnlChartEventType.Size = new Size(392, 444);
        pnlChartEventType.TabIndex = 1;
        // 
        // plotViewEventType
        // 
        plotViewEventType.Dock = DockStyle.Fill;
        plotViewEventType.Location = new Point(0, 0);
        plotViewEventType.Name = "plotViewEventType";
        plotViewEventType.PanCursor = Cursors.Hand;
        plotViewEventType.Size = new Size(392, 444);
        plotViewEventType.TabIndex = 0;
        plotViewEventType.Text = "Event Type";
        plotViewEventType.ZoomHorizontalCursor = Cursors.SizeWE;
        plotViewEventType.ZoomRectangleCursor = Cursors.SizeNWSE;
        plotViewEventType.ZoomVerticalCursor = Cursors.SizeNS;
        // 
        // pnlChartCityDistribution
        // 
        pnlChartCityDistribution.Controls.Add(plotViewCityDistribution);
        pnlChartCityDistribution.Dock = DockStyle.Fill;
        pnlChartCityDistribution.Location = new Point(3, 3);
        pnlChartCityDistribution.Name = "pnlChartCityDistribution";
        pnlChartCityDistribution.Size = new Size(392, 422);
        pnlChartCityDistribution.TabIndex = 0;
        // 
        // plotViewCityDistribution
        // 
        plotViewCityDistribution.Dock = DockStyle.Fill;
        plotViewCityDistribution.Location = new Point(0, 0);
        plotViewCityDistribution.Name = "plotViewCityDistribution";
        plotViewCityDistribution.PanCursor = Cursors.Hand;
        plotViewCityDistribution.Size = new Size(392, 422);
        plotViewCityDistribution.TabIndex = 0;
        plotViewCityDistribution.Text = "City Distribution";
        plotViewCityDistribution.ZoomHorizontalCursor = Cursors.SizeWE;
        plotViewCityDistribution.ZoomRectangleCursor = Cursors.SizeNWSE;
        plotViewCityDistribution.ZoomVerticalCursor = Cursors.SizeNS;
        // 
        // pnlChartAgeGroupDistribution
        // 
        pnlChartAgeGroupDistribution.Controls.Add(plotViewAgeGroupDistribution);
        pnlChartAgeGroupDistribution.Dock = DockStyle.Fill;
        pnlChartAgeGroupDistribution.Location = new Point(401, 3);
        pnlChartAgeGroupDistribution.Name = "pnlChartAgeGroupDistribution";
        pnlChartAgeGroupDistribution.Size = new Size(392, 422);
        pnlChartAgeGroupDistribution.TabIndex = 1;
        // 
        // plotViewAgeGroupDistribution
        // 
        plotViewAgeGroupDistribution.Dock = DockStyle.Fill;
        plotViewAgeGroupDistribution.Location = new Point(0, 0);
        plotViewAgeGroupDistribution.Name = "plotViewAgeGroupDistribution";
        plotViewAgeGroupDistribution.PanCursor = Cursors.Hand;
        plotViewAgeGroupDistribution.Size = new Size(392, 422);
        plotViewAgeGroupDistribution.TabIndex = 0;
        plotViewAgeGroupDistribution.Text = "Age Group Distribution";
        plotViewAgeGroupDistribution.ZoomHorizontalCursor = Cursors.SizeWE;
        plotViewAgeGroupDistribution.ZoomRectangleCursor = Cursors.SizeNWSE;
        plotViewAgeGroupDistribution.ZoomVerticalCursor = Cursors.SizeNS;
        // 
        // pnlChartMonthlyVisitsTrend
        // 
        pnlChartMonthlyVisitsTrend.Controls.Add(plotViewMonthlyVisitsTrend);
        pnlChartMonthlyVisitsTrend.Dock = DockStyle.Fill;
        pnlChartMonthlyVisitsTrend.Location = new Point(799, 3);
        pnlChartMonthlyVisitsTrend.Name = "pnlChartMonthlyVisitsTrend";
        pnlChartMonthlyVisitsTrend.Size = new Size(792, 422);
        pnlChartMonthlyVisitsTrend.TabIndex = 2;
        // 
        // plotViewMonthlyVisitsTrend
        // 
        plotViewMonthlyVisitsTrend.Dock = DockStyle.Fill;
        plotViewMonthlyVisitsTrend.Location = new Point(0, 0);
        plotViewMonthlyVisitsTrend.Name = "plotViewMonthlyVisitsTrend";
        plotViewMonthlyVisitsTrend.PanCursor = Cursors.Hand;
        plotViewMonthlyVisitsTrend.Size = new Size(792, 422);
        plotViewMonthlyVisitsTrend.TabIndex = 0;
        plotViewMonthlyVisitsTrend.Text = "Monthly Visits Trend";
        plotViewMonthlyVisitsTrend.ZoomHorizontalCursor = Cursors.SizeWE;
        plotViewMonthlyVisitsTrend.ZoomRectangleCursor = Cursors.SizeNWSE;
        plotViewMonthlyVisitsTrend.ZoomVerticalCursor = Cursors.SizeNS;
        // 
        // pnlChartPantryDayVolume (in pnlServices)
        // 
        // 
        pnlChartPantryDayVolume.Controls.Add(plotViewPantryDayVolume);
        pnlChartPantryDayVolume.Location = new Point(0, 450);
        pnlChartPantryDayVolume.Name = "pnlChartPantryDayVolume";
        pnlChartPantryDayVolume.Size = new Size(1600, 336);
        pnlChartPantryDayVolume.TabIndex = 1;
        pnlChartPantryDayVolume.Dock = DockStyle.Fill;
        // 
        // plotViewPantryDayVolume
        // 
        plotViewPantryDayVolume.Dock = DockStyle.Fill;
        plotViewPantryDayVolume.Location = new Point(0, 0);
        plotViewPantryDayVolume.Name = "plotViewPantryDayVolume";
        plotViewPantryDayVolume.PanCursor = Cursors.Hand;
        plotViewPantryDayVolume.Size = new Size(1588, 300);
        plotViewPantryDayVolume.TabIndex = 0;
        plotViewPantryDayVolume.Text = "Pantry Day Volume by Event";
        plotViewPantryDayVolume.ZoomHorizontalCursor = Cursors.SizeWE;
        plotViewPantryDayVolume.ZoomRectangleCursor = Cursors.SizeNWSE;
        plotViewPantryDayVolume.ZoomVerticalCursor = Cursors.SizeNS;
        // 
        // btnExportPdf
        // 
        btnExportPdf.Location = new Point(1400, 22);
        btnExportPdf.Name = "btnExportPdf";
        btnExportPdf.Size = new Size(90, 23);
        btnExportPdf.TabIndex = 6;
        btnExportPdf.Text = "Export PDF";
        btnExportPdf.UseVisualStyleBackColor = true;
        btnExportPdf.Click += BtnExportPdf_Click;
        btnExportPdf.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        // 
        // btnPrint
        // 
        btnPrint.Location = new Point(1496, 22);
        btnPrint.Name = "btnPrint";
        btnPrint.Size = new Size(90, 23);
        btnPrint.TabIndex = 7;
        btnPrint.Text = "Print";
        btnPrint.UseVisualStyleBackColor = true;
        btnPrint.Click += BtnPrint_Click;
        btnPrint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        // 
        // StatsForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1624, 980);
        MinimumSize = new Size(1624, 980);
        Controls.Add(pnlServices);
        Controls.Add(pnlDemographics);
        Controls.Add(pnlSummaryCards);
        Controls.Add(grpDateRange);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = true;
        Name = "StatsForm";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Yearly Statistics";
        Load += StatsForm_Load;
        grpDateRange.ResumeLayout(false);
        grpDateRange.PerformLayout();
        pnlSummaryCards.ResumeLayout(false);
        pnlCardTotalActiveHouseholds.ResumeLayout(false);
        pnlCardTotalActiveHouseholds.PerformLayout();
        pnlCardTotalPeople.ResumeLayout(false);
        pnlCardTotalPeople.PerformLayout();
        pnlCardCompletedServices.ResumeLayout(false);
        pnlCardCompletedServices.PerformLayout();
        pnlCardUniqueHouseholdsServed.ResumeLayout(false);
        pnlCardUniqueHouseholdsServed.PerformLayout();
        pnlDemographics.ResumeLayout(false);
        tableLayoutDemographics.ResumeLayout(false);
        pnlChartCityDistribution.ResumeLayout(false);
        pnlChartAgeGroupDistribution.ResumeLayout(false);
        pnlChartRaceDistribution.ResumeLayout(false);
        pnlChartVeteranDistribution.ResumeLayout(false);
        pnlChartDisabilityDistribution.ResumeLayout(false);
        pnlServices.ResumeLayout(false);
        tableLayoutServicesRow1.ResumeLayout(false);
        pnlChartVisitType.ResumeLayout(false);
        pnlChartEventType.ResumeLayout(false);
        pnlChartMonthlyVisitsTrend.ResumeLayout(false);
        pnlChartPantryDayVolume.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private GroupBox grpDateRange;
    private Label lblDateRange;
    private ComboBox cmbDateRange;
    private Label lblStartDate;
    private DateTimePicker dtpStartDate;
    private Label lblEndDate;
    private DateTimePicker dtpEndDate;
    private Panel pnlSummaryCards;
    private Panel pnlCardTotalActiveHouseholds;
    private Label lblCardTotalActiveHouseholds;
    private Label lblCardTotalActiveHouseholdsValue;
    private Panel pnlCardTotalPeople;
    private Label lblCardTotalPeople;
    private Label lblCardTotalPeopleValue;
    private Panel pnlCardCompletedServices;
    private Label lblCardCompletedServices;
    private Label lblCardCompletedServicesValue;
    private Panel pnlCardUniqueHouseholdsServed;
    private Label lblCardUniqueHouseholdsServed;
    private Label lblCardUniqueHouseholdsServedValue;
    private Panel pnlDemographics;
    private TableLayoutPanel tableLayoutDemographics;
    private Panel pnlChartCityDistribution;
    private OxyPlot.WindowsForms.PlotView plotViewCityDistribution;
    private Panel pnlChartAgeGroupDistribution;
    private OxyPlot.WindowsForms.PlotView plotViewAgeGroupDistribution;
    private Panel pnlChartRaceDistribution;
    private OxyPlot.WindowsForms.PlotView plotViewRaceDistribution;
    private Panel pnlChartVeteranDistribution;
    private OxyPlot.WindowsForms.PlotView plotViewVeteranDistribution;
    private Panel pnlChartDisabilityDistribution;
    private OxyPlot.WindowsForms.PlotView plotViewDisabilityDistribution;
    private Panel pnlServices;
    private TableLayoutPanel tableLayoutServicesRow1;
    private Panel pnlChartVisitType;
    private OxyPlot.WindowsForms.PlotView plotViewVisitType;
    private Panel pnlChartEventType;
    private OxyPlot.WindowsForms.PlotView plotViewEventType;
    private Panel pnlChartMonthlyVisitsTrend;
    private OxyPlot.WindowsForms.PlotView plotViewMonthlyVisitsTrend;
    private Panel pnlChartPantryDayVolume;
    private OxyPlot.WindowsForms.PlotView plotViewPantryDayVolume;
    private Button btnPageDemographics;
    private Button btnPageServices;
    private Button btnExportPdf;
    private Button btnPrint;
}
