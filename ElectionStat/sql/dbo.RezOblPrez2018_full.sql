create function dbo.RezOblPrez2018()--результаты выборов президента 2018 по ТИКам Новгородской области
returns table
as
return (select rayon, sum(p1) as p1, sum(p7+p8) as p78,
sum(k2_c) as k2_c, ROUND(100*cast(sum(k2_c) as decimal(10,4))/cast(sum(p7+p8) as decimal(10,4)), 2) as k2_p, 
sum(k3_c) as k3_c, ROUND(100*cast(sum(k3_c) as decimal(10,4))/cast(sum(p7+p8) as decimal(10,4)), 2) as k3_p,	
sum(k4_c) as k4_c, ROUND(100*cast(sum(k4_c) as decimal(10,4))/cast(sum(p7+p8) as decimal(10,4)), 2) as k4_p, 
sum(k1_c + k5_c + k6_c + k7_c + k8_c) as k_c, 
ROUND(100*cast(sum(k1_c + k5_c + k6_c + k7_c + k8_c) as decimal(10,4))/cast(sum(p7+p8) as decimal(10,4)), 2) as k_p,
avg(CAST(lat AS DECIMAL(10,8))) as lat, 
avg(CAST(lon AS DECIMAL(10,8))) as lon, 
ROUND(100*cast(sum(p7+p8) as decimal(6,0))/cast(sum(p1) as decimal(6,0)), 2) as vote,
ROUND(100*cast(sum(k2_c) as decimal(10,4))/cast(sum(p1) as decimal(10,4)),0) as k2_w, 
ROUND(100*cast(sum(k3_c) as decimal(10,4))/cast(sum(p1) as decimal(10,4)),0) as k3_w, 
ROUND(100*cast(sum(k4_c) as decimal(10,4))/cast(sum(p1) as decimal(10,4)),0) as k4_w, 
ROUND(100*cast(sum(k1_c + k5_c + k6_c + k7_c + k8_c) as decimal(10,4))/cast(sum(p1) as decimal(10,4)),0) as k_w
		from obl_uiks_18032018 
		group by rayon)