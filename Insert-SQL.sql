INSERT INTO PricingRule Values('A',50,3,20)
INSERT INTO PricingRule Values('B',30,2,15)
INSERT INTO PricingRule Values('C',20,null,null)
INSERT INTO PricingRule Values('D',15,null,null)

select * from PricingRule

update PricingRule set Item = 'A', UnitPrice = 50, DiscountPriceUnits = 2, DiscountPrice = 10 where itemid = 5



--A->5> 130+90 = 220

--A->6>260

--USE BOTH RULES
--A>6> 130+ +90+50 = 270
































--EF code first commands
--Add-Migration <NameO:Initial>
--Update-Database