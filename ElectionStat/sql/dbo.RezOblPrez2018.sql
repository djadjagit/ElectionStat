CREATE function RezOblPrez2018()--результаты выборов президента 2018 по ТИКам Новгородской области
returns table
as
return (select rayon, sum(p1) as p1, sum(p7+p8) as p78, sum(k2_c) as k2_c, CAST(100*sum(k2_c)/sum(p7+p8) AS DECIMAL(6,2)) as k2_p, sum(k3_c) as k3_c, 
			CAST(100*sum(k3_c)/sum(p7+p8) AS DECIMAL(6,2)) as k3_p,	sum(k4_c) as k4_c, CAST(100*sum(k4_c)/sum(p7+p8) AS DECIMAL(6,2)) as k4_p, 
			sum(k1_c + k5_c + k6_c + k7_c + k8_c) as k_c, CAST(100*sum(k1_c + k5_c + k6_c + k7_c + k8_c)/sum(p7+p8) AS DECIMAL(6,2)) as k_p,
            avg(CAST(lat AS DECIMAL(10,8))) as lat, avg(CAST(lon AS DECIMAL(10,8))) as lon, CAST(100*sum(p1)/sum(p7+p8) AS DECIMAL(6,2)) as vote 
		from obl_uiks_18032018 
		group by rayon)