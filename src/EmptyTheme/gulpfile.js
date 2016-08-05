var gulp = require('gulp');
var clean = require('gulp-dest-clean');

gulp.task('default', ['copy']);

gulp.task('copy', ['copy:css', 'copy:js', 'copy:fonts']);

gulp.task('copy:css', ['cleanup'], function () {
    return gulp.src('./node_modules/bootstrap/dist/css/*.min.css').pipe(gulp.dest('./wwwroot/css')).pipe(clean('./wwwroot/css'));
});

gulp.task('copy:fonts', ['cleanup'], function () {
    return gulp.src('./node_modules/bootstrap/dist/fonts/*').pipe(gulp.dest('./wwwroot/fonts')).pipe(clean('./wwwroot/fonts'));
});

gulp.task('copy:js', ['cleanup'], function () {
    return gulp.src('./node_modules/bootstrap/dist/js/*.min.js').pipe(gulp.dest('./wwwroot/js')).pipe(clean('./wwwroot/js'));
});
