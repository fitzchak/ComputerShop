ComputerShop
============

required ecmaScript 5 compatible browser
 IE8+, Chrome, Firefox, Opera

Use the Index.html to view the public interface and Admin.html to do the administration.

This is taken as the proofOfConcept on this slice of technologies/libraries

SERVER
- MSSql
  - with tables, stps for insert/update/delete
- Entity Framework Code First
  - to provide ORM mapping
  - to generate database, including STPs (info taken from EF metadata)
- System.Web.Http.ApiController (MVC 4) - supported with Breeze.WebApi
  - provides set of operations and IQueryable interface

CLIENT
- breeze.js
  - to provide communication and data exchange in asynchronous manner
- backbone.js
  - MVVM library to support the view-model binding
- jQuery
  - to manipulate DOM, provide autocomplete box

notes:

Database incuding stored procedures is created on first start of application.
To have some entries in database run ComputerShopDataTest.InitDB test in ComputerShop.Data.Test project.
