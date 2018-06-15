# ParrotWingsMVC
The application is for Parrot Wings (PW, “internal money”) transfer between system users. 
Any person on Earth can register with the service for free, providing their Name, valid
email and password.
When a new user registers, the System will verify, that the user has provided a unique (not previously
registered in the system) email, and also provided name and a password. These 3 fields are
mandatory. Password is to be typed twice for justification. 
On successful registration every User will be awarded with 500 (five hundred) PW starting balance.
The system will allow users to perform the following operations:
1) See their Name and current PW balance always on screen
2) Create a new transaction. 
To make a new transaction (PW payment) a user will
a) Choose the recipient by querying the User list by name (autocomplete).
b) When a recipient chosen, entering the PW transaction amount. The system will check
that the transaction amount is not greater than the current user balance.
c) Committing the transaction. Once transaction succeeded, the recipient account will be
credited (PW++) by the entered amount of PW, and the payee account debited (PW--)
for the same amount of PW. The system shall display PW balance changes immediately
to the user.

Technical requirements

a. MS SQL Server 2012
b. IIS
c. Web API
d. Entity framework 6
