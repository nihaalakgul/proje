// /** @type {import('tailwindcss').Config} */
// module.exports = {
//   purge: ["./src/**/*.{js,jsx,ts,tsx}", "./public/index.html"],
//   content: [],
//   theme: {
//     extend: {
//       colors: {
//         slateblue: "#6A5ACD",
//         seagreen: "#2E8B57",
//         "rgba-255": "rgba(255, 255, 255, 0.1)",
//       },
//       height: {
//         450: "450px",
//       },
//       width: {
//         420: "420px",
//       },
//     },
//   },
//   variants: {
//     extend: {},
//   },
//   plugins: [],
// };

module.exports = {
  content: ["./src/**/*.{js,jsx,ts,tsx}"],
  theme: {
    extend: {
      colors: {
        slateblue: "#6A5ACD",
        seagreen: "#2E8B57",
        "rgba-255": "rgba(255, 255, 255, 0.1)",
      },
      height: {
        450: "450px",
      },
      width: {
        420: "420px",
      },
    },
  },
  plugins: [],
};
