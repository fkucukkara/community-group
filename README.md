# community groups

The project is developed since 2022.

Community Groups is a cross-platform, Docker supported project running on the ASP.NET Core 5 with MSSQL.

The project can run on Docker (linux or windows) or IIS.

Https is also supported.

## purpose and use cases

It contains restful services to create a community on any base.

- Create a person entry with the following information:
	- First name
	- Last name
	- Email (unique)
	- Occupation (optional)
- Edit a person entry
- Delete a person entry
- Import a csv file for bulk creating person entries
- Search, order and paginate person entries
- Create a community group with a community name
- Edit a community group name
- Delete a community group
- Assign users to a community group
- Remove users from a community group
- Return a community group with the people contained in it
- Users can register and log in with email and password
- People and community groups must belong to the logged-in user

## swagger

**https://localhost:44308/swagger/index.html** is the adress where you can see the service list.
Authentication is required before using.

## security

JWT is used on authentication.
Login path and Demo credentials are given below:

**login path:** /api/v1/Login/login
**username:** crea
**password:** crea

After giving the credentions you should give the token to the Authorize section of the swagger. Then, you can freely use the services.

## architecture

- .NET Core 5 is used as framework
- JWT based authentication is used on the security layer
- Generic Repository Pattern is used on the data access layer
- Middleware is used as central exception handling
- URL based versioning is used
- EntityFramework CodeFirst is used to sync the database **add-migration** and **update-database** commands are necessary