# JJSolutions.TableList.AspNetCore.TagHelper

TableList render with TAG Helper for .Net Framework and Asp.Net Core 1.1

Create a table with pagination, search and many other features and customizations.

Sample to use:

<strong>Test Class Customer</strong>

```c#
    public class Customer
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
```
<strong>Controller sample: (CustomerController.cs)</strong>
<br>
```c#
        public IActionResult Index(string searchString, int? page, string returnUrl = null, string sortOrder = "CustomerId", string sortDirection = "asc")
        {
            if (!string.IsNullOrEmpty(returnUrl))
                return RedirectToReturnUrl(returnUrl);

            var query = GetCustomers().AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                // search
                query = query.Where(x =>
                   x.Name.Contains(searchString) ||
                   x.Email.Contains(searchString) ||
                   x.Phone.Contains(searchString));
            }

            // OrderBy
            var param = sortOrder;
            var propertyInfo = typeof(Customer).GetProperty(param);
            query = sortDirection == "asc" ? query.OrderBy(x => propertyInfo.GetValue(x, null)) : query.OrderByDescending(x => propertyInfo.GetValue(x, null));

            PagedList<Customer> pagedList = query
               .ToPagedList(page ?? 1, 10);

            ViewBag.CurrentPage = page ?? 1;
            ViewBag.PageCount = pagedList.PageCount;
            ViewBag.RecordCount = pagedList.TotalCount;
            ViewBag.SearchString = searchString;
            ViewBag.SortOrder = sortOrder;
            ViewBag.SortDirection = sortDirection;

            return View(pagedList);
        }
        
        // create a sample list of Customes
        private List<Customer> GetCustomers()
        {
            var ret = new List<Customer>
            {
                new Customer
                {
                    CustomerId = 1,
                    Name = "Customer Sample 01",
                    Phone = "1111-1111",
                    Email = "customer@customer.com"
                },
                new Customer
                {
                    CustomerId = 2,
                    Name = "Customer Sample 02",
                    Phone = "2222-2222",
                    Email = ""
                },
                new Customer
                {
                    CustomerId = 3,
                    Name = "Customer Sample 03",
                    Phone = "3333-3333",
                    Email = "customer2@customer.com"
                },
                new Customer
                {
                    CustomerId = 4,
                    Name = "Customer Sample 04",
                    Phone = "4444-4444",
                    Email = ""
                },
                new Customer
                {
                    CustomerId = 5,
                    Name = "Customer Sample 05",
                    Phone = "5555-5555",
                    Email = "customer5@customer.com"
                },
                new Customer
                {
                    CustomerId = 6,
                    Name = "Customer Sample 06",
                    Phone = "6666-6666",
                    Email = "customer6@customer.com"
                },
                new Customer
                {
                    CustomerId = 7,
                    Name = "Customer Sample 07",
                    Phone = "7777-7777",
                    Email = "customer7@customer.com"
                },
                new Customer
                {
                    CustomerId = 8,
                    Name = "Customer Sample 08",
                    Phone = "8888-8888",
                    Email = ""
                },
                new Customer
                {
                    CustomerId = 9,
                    Name = "Customer Sample 09",
                    Phone = "",
                    Email = "customer8@customer.com"
                },
                new Customer
                {
                    CustomerId = 10,
                    Name = "Customer Sample 10",
                    Phone = "9999-9999",
                    Email = ""
                },
                new Customer
                {
                    CustomerId = 11,
                    Name = "Customer Sample 11",
                    Phone = "1234-1234",
                    Email = "custome11r@customer.com"
                },
                new Customer
                {
                    CustomerId = 12,
                    Name = "Customer Sample 12",
                    Phone = "456-789",
                    Email = "customer12@customer.com"
                },
                new Customer
                {
                    CustomerId = 13,
                    Name = "Customer Sample 13",
                    Phone = "7894-1234",
                    Email = ""
                },
                new Customer
                {
                    CustomerId = 14,
                    Name = "Customer Sample 14",
                    Phone = "1956-4678",
                    Email = "customer14@customer.com"
                },
            };

            return ret;
        }    

        protected IActionResult RedirectToReturnUrl(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

```
<br><br>
<strong>Important!</strong>
<br><br>
Install nuget package JJSolutions.TableList.AspNet.TagHelper or JJSolutions.TableList.AspNetCore.TagHelper and register the Tag Helper insert the line below into <strong>_ViewImports.cshtml</strong> file:
```c#
@addTagHelper "*, JJSolutions.TableList.AspNet"
```
If you choose AspNetCore package, add this line:
```c#
@addTagHelper "*, JJSolutions.TableList.AspNetCore"
```
<br><br>
<strong>View usage sample:</strong>
<br>
The model must be a List of Entity that Controller returns <strong>@model List&lt;Customer&gt;</strong>
<br><br>
The returnUrl is a parameter that need to be buid and encoded with ViewBag variables returned by Controller
<br>
```c#
        @model List<Customer>;
        @{
            ViewData["Title"] = "Sample Application";
            var returnUrl = $"{Url.Action("Index")}?page={ViewBag.CurrentPage}&searchString={ViewBag.SearchString}&sortOrder={ViewBag.SortOrder}&sortDirection={ViewBag.SortDirection}";
         }
```
<br><br>
And after that, the TableList can be written
```html
   <h1>Sample Application</h1>
    
    <jjsolutions-table-list model="@Model" asp-controller="Customer" asp-action="Index" return-url="@returnUrl">
        <table-columns>
            <table-column asp-for="CustomerId" header-style="width: 100px;" custom-link="/Controller/Details/{0}" />
            <table-column asp-for="Name" />
            <table-column asp-for="Email" header-style="width: 250px;" custom-link="mailto:{0}" />
            <table-column asp-for="Phone" no-sort="true" />
        </table-columns>
        <table-buttons header-style="width: 100px;">
            <table-button title="Edit Customer" icon-class="fa fa-edit" asp-action="Edit" asp-route-id="CustomerId" on-click="showProgress();" />
            <table-button title="Delete Customer" icon-class="fa fa-times" class="text-danger" asp-action="Delete" asp-route-id="CustomerId" on-click="showProgress();" />
        </table-buttons>
        <table-settings>
            <search-settings search-string="@ViewBag.SearchString" record-count="@ViewBag.RecordCount" />
            <sort-settings sort-order="@ViewBag.SortOrder" sort-direction="@ViewBag.SortDirection" />
            <legend-setttings title="Legend" />
            <pagination-setttings current-page="@ViewBag.CurrentPage" page-count="@ViewBag.PageCount" />
        </table-settings>
    </jjsolutions-table-list>
```
<br>
<h2>Requirements</h2>
- Install Font-Awesome in bower packages<br/>
- Reference font-awesome.css in _Layout.chtml<br/>
<br>
<h2>Nuget Package</h2>
This component is available in nuget package<br/>
<strong>JJSolutions.TableList.AspNet.TagHelper</strong><br>
<strong>JJSolutions.TableList.AspNetCore.TagHelper</strong>
<br>

