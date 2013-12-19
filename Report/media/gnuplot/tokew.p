#Session -1600301272

set output "tokew.eps"
set term postscript
set title "Acceleration Graph"
set xlabel "Time (ms)"
set ylabel "Acceleration (g)"
set autoscale
plot "rightstepdata.dat" title "Right Step" with lines lw 5 lc rgb 'red', "leftstepdata.dat" title "Left Step" with lines lw 5 lt 1 lc rgb 'blue'
