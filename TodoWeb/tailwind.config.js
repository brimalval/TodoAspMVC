const defaultTheme = require('tailwindcss/defaultTheme');

module.exports = {
  content: [
      "./Views/**/*.cshtml",
  ],
  theme: {
    screens: {
      'xs': '128px',
      ...defaultTheme.screens,
    },
    extend: {
      colors: {
        primary: '#AB47BC',
        secondary: '#D500F9',
        danger: '#EF4444'
      }
    },
  },
  plugins: [],
}