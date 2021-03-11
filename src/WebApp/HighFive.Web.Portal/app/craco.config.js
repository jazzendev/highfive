const path = require("path");

module.exports = {
  webpack: {
    alias: {
      "Services": path.resolve(__dirname, "src/services/"),
      "Utilities": path.resolve(__dirname, "src/utilities/"),
    },
    configure: {
      externals: {
        // global app config object
        config: JSON.stringify({
          apiUrl: "http://localhost:63295",
        }),
      },
    },
  },
};
