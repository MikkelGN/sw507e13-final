#Session ?????

set output "compfilter.eps"
set term postscript
set title "Gyroscope Graph"
set xlabel "Time (ms)"
set ylabel "Angle (rad)"
set autoscale
plot "compfilterdata.dat" using 1:2 title "Complementary-filter Data" with lines lc rgb 'red', "rawdata.dat" using 1:2 title "Raw Data" with lines lc rgb 'blue'
