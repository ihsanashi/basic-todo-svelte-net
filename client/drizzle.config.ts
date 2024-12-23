import { defineConfig } from 'drizzle-kit';

export default defineConfig({
	schema: './src/lib/server/db/schema.ts',

	verbose: true,
	strict: true,
	dialect: 'mysql'
});
