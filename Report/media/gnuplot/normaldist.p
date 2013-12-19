#Session ?????

set output "normaldist.eps"
set term postscript enhanced
set title "Normal Distribution Graph"
set xlabel "Acceleration (g)"
set ylabel ""
set autoscale
set arrow from 0,0 to 0,140 nohead lt 2 lc rgb 'black'
plot "normaldist.dat" using 1:2 title "Normal Distribution" with lines lc rgb 'blue', "accelerationdist.dat" using 1:2 title "Recorded Data" with points pt 3 lc rgb 'red', "sigmacalcdata.dat" title "Calculated {/Symbol s}" with points pt 7 lc rgb 'green', "calcmudata.dat" title "Calculated {/Symbol m}"with points pt 7 lc rgb 'blue'