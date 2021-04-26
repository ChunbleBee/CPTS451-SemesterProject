CREATE OR REPLACE FUNCTION myDistance(
							alat double precision,
							alng double precision,
							blat double precision,
							blng double precision)
	RETURNS double precision
	AS 'SELECT sqrt(power(alat - blat, 2) + power(alng - blng, 2)) as distance;'
LANGUAGE SQL;