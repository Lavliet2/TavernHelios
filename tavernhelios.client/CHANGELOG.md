
# Changelog

## [1.0.0] - 2025-01-31
### Added
- Создано API MenuServis для управления блюдами и меню.


## [0.0.0] - 2025-01-20



This file explains how Visual Studio created the project.

The following tools were used to generate this project:
- create-vite

The following steps were used to generate this project:
- Create react project with create-vite: `npm init --yes vite@latest tavernhelios.client -- --template=react-ts`.
- Update `vite.config.ts` to set up proxying and certs.
- Add `@type/node` for `vite.config.js` typing.
- Update `App` component to fetch and display weather information.
- Create project file (`tavernhelios.client.esproj`).
- Create `launch.json` to enable debugging.
- Add project to solution.
- Update proxy endpoint to be the backend server endpoint.
- Add project to the startup projects list.
- Write this file.